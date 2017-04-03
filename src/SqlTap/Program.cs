using System;
using ServiceStack.Text;
using SqlTap.SqlServer;
using System.Linq;

namespace SqlTap
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var adapter = new DatabaseAdapter("server=127.0.0.1;database=content-bank;uid=local_admin;pwd=password");
            var database = adapter.GetDatabase();

            var table = database.Tables.First();
            var sql = adapter.ComposeSql(table);

            Console.WriteLine(sql);

            Console.ReadLine();
        }
    }
}