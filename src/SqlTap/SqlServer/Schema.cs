using System.Collections.Generic;

namespace SqlTap.SqlServer
{
    class Schema
    {
        public Schema()
        {
            Tables = new List<Table>();
        }

        public string Name { get; set; }
        public string Owner { get; set; }
        public List<Table> Tables { get; set; }
    }
}