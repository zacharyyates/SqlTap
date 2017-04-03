namespace SqlTap.SqlServer
{
    internal class Column
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public Table Table { get; set; }
        public string TableName { get; set; }
    }
}