using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sales.Extensions
{
    public static class SqlDataReaderExtensions
    {
        public static string? GetStringSafe(this SqlDataReader reader, string columnName)
        {
            int ordinal = reader.GetOrdinal(columnName);

            if (reader.IsDBNull(ordinal))
                return null;

            return reader.GetString(ordinal);
        }
        public static int? GetIntSafe(this SqlDataReader reader,string columnName)
        {
            int ordinal = reader.GetOrdinal(columnName);

            if (reader.IsDBNull(ordinal))
                return null;

            return reader.GetInt32(ordinal);
        }
        public static double? GetDoubleSafe(this SqlDataReader reader,string columnName)
        {
            int ordinal = reader.GetOrdinal(columnName);

            if (reader.IsDBNull(ordinal))
                return null;

            return reader.GetDouble(ordinal);
        }
        public static decimal? GetDecimalSafe(this SqlDataReader reader,string columnName)
        {
            int ordinal = reader.GetOrdinal(columnName);

            if (reader.IsDBNull(ordinal))
                return null;

            return reader.GetDecimal(ordinal);
        }

        public static DateTime? GetDateTimeSafe(this SqlDataReader reader, string columnName)
        {
            int ordinal = reader.GetOrdinal(columnName);

            if (reader.IsDBNull(ordinal))
                return null;

            return reader.GetDateTime(ordinal);
        }

        public static bool? GetBoolSafe(this SqlDataReader reader, string columnName)
        {
            int ordinal = reader.GetOrdinal(columnName);

            if (reader.IsDBNull(ordinal))
                return null;

            return reader.GetBoolean(ordinal);
        }
    }
}
