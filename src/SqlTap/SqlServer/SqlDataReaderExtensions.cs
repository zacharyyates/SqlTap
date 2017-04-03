using System.Data.SqlClient;

namespace SqlTap.SqlServer
{
    internal static class SqlDataReaderExtensions
    {
        public static string GetString(this SqlDataReader reader, string column)
        {
            return reader.GetString(reader.GetOrdinal(column));
        }
    }
}