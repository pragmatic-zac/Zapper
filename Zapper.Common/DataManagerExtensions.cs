using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Zapper.Common
{
    // WIP: move everything to here - extension methods rather than a standalone class
    public static class DataManagerExtensions
    {
        public static int Execute(this SqlConnection connection, string sql, CommandType commandType)
        {
            using (SqlConnection conn = connection)
            {
                SqlCommand command = new SqlCommand(sql, conn);
                conn.Open();
                command.CommandType = commandType;
                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected;
            }
        }
    }
}
