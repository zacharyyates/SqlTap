using System.Collections.Generic;

namespace SqlTap.SqlServer
{
    class Table
    {
        public Table()
        {
            Columns = new List<Column>();
        }

        public string Name { get; set; }
        public string Type { get; set; }
        public Schema Schema { get; set; }
        public string SchemaName { get; set; }
        public List<Column> Columns { get; set; }
    }
}