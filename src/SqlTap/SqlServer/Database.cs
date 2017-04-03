using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SqlTap.SqlServer
{
    class Database
    {
        public Database()
        {
            Schemas = new List<Schema>();
            Tables = new List<Table>();
        }

        public string Name { get; set; }
        public List<Schema> Schemas { get; set; }
        public List<Table> Tables { get; set; }
    }
}
