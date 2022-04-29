using System;
using Microsoft.Data.SqlClient;

namespace Dose.Utils
{
    public static class DbUtils
    {
        public static string GetNullableString(SqlDataReader reader, string column)
        {
            var ordinal = reader.GetOrdinal(column);
            if (reader.IsDBNull(ordinal))
            {
                return null;
            }
            return reader.GetString(ordinal);
        }
    }
}
