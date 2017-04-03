using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using ServiceStack.Text;
using System.Text;

namespace SqlTap.SqlServer
{
    internal class DatabaseAdapter : IDisposable
    {
        private readonly SqlConnection _connection;

        private const string GetDatabaseSchemasCommand = @"select * from information_schema.schemata";
        private const string GetDatabaseTablesCommand = @"select * from information_schema.tables";
        private const string GetDatabaseColumnsCommand = @"select * from information_schema.columns";

        public DatabaseAdapter(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public Database GetDatabase()
        {
            var schemas = new Dictionary<string, Schema>();

            _connection.Open();
            using (var command = new SqlCommand(GetDatabaseSchemasCommand, _connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var schema = new Schema();
                    schema.Name = reader.GetString("SCHEMA_NAME");
                    schema.Owner = reader.GetString("SCHEMA_OWNER");
                    schemas.Add(schema.Name, schema);
                }
            }
            _connection.Close();

            var tables = new Dictionary<string, Table>();

            _connection.Open();
            using (var command = new SqlCommand(GetDatabaseTablesCommand, _connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var table = new Table();
                    table.Name = reader.GetString("TABLE_NAME");
                    table.Type = reader.GetString("TABLE_TYPE");

                    var schemaName = reader.GetString("TABLE_SCHEMA");
                    table.SchemaName = schemaName;
                    tables.Add(table.Name, table);

                    var schema = schemas[schemaName];
                    schema.Tables.Add(table);

                    // leaving off the ref to the container because of stackoverflow exceptions while PrintDumping()
                    //table.Schema = schema;
                }
            }
            _connection.Close();

            _connection.Open();
            using (var command = new SqlCommand(GetDatabaseColumnsCommand, _connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var column = new Column();

                    var schemaName = reader.GetString("TABLE_SCHEMA");
                    var tableName = reader.GetString("TABLE_NAME");

                    column.Name = reader.GetString("COLUMN_NAME");
                    column.Type = reader.GetString("DATA_TYPE");
                    column.TableName = tableName;

                    var table = tables.Single(x => x.Value.Name == tableName && x.Value.SchemaName == schemaName).Value;
                    table.Columns.Add(column);
                    
                    // leaving off the ref to the container because of stackoverflow exceptions while PrintDumping()
                    //column.Table = table;
                }
            }
            _connection.Close();

            return new Database()
            {
                Schemas = schemas.Select(x => x.Value).ToList(),
                Tables = tables.Select(x => x.Value).ToList()
            };
        }

        public string ComposeSql(Table table)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            var output = new StringBuilder();
            output.AppendLine($"create table [{table.SchemaName}].[{table.Name}] (");

            var firstLine = true;
            foreach(var column in table.Columns)
            {
                output.AppendLine($" {(firstLine ? ' ' : ',')} [{column.Name}] {column.Type}");
                firstLine = false;
            }
            output.AppendLine(");");

            return output.ToString();
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}