using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Zapper.Tests.SqlExecutor
{
    // a first pass at a quick reusable executor class for reuse throughout test methods
    public class SqlExecutor
    {
        // connection string
        private string connectionString = "Server=.;Database=ZapperDB;Trusted_Connection=True;";

        // for inserting data
        public int Execute(string sql)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sql, conn);
                conn.Open();
                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected;
            }
        }

    }
}
