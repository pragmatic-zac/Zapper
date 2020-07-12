using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Zapper.Common
{
    // make this an extension on IDbConnection
    public class DataManagerBase
    {
        private List<T> ToList<T>(SqlDataReader reader)
        {
            List<T> toReturn = new List<T>();
            T entity;
            Type type = typeof(T);
            PropertyInfo col;
            List<PropertyInfo> columns = new List<PropertyInfo>();
            PropertyInfo[] props = type.GetProperties();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                col = props.FirstOrDefault(c => c.Name == reader.GetName(i));
                if (col != null)
                {
                    columns.Add(col);
                }
            }

            while (reader.Read())
            {
                entity = Activator.CreateInstance<T>();

                // loop thru columns and assign data
                for (int i = 0; i < columns.Count; i++)
                {
                    if (reader[columns[i].Name].Equals(DBNull.Value))
                    {
                        columns[i].SetValue(entity, null, null);
                    }
                    else
                    {
                        columns[i].SetValue(entity, reader[columns[i].Name], null);
                    }
                }
                toReturn.Add(entity);
            }

            return toReturn;
        }

        public List<T> Query<T>(string sql, string connectionString, CommandType commandType)
        {
            List<T> toReturn = new List<T>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    command.CommandType = commandType;

                    using (SqlDataReader dataReader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        toReturn = ToList<T>(dataReader);
                    }
                }
            }

            return toReturn;
        }

        // TODO: remove List from this
        public T QuerySingle<T>(string sql, string connectionString)
        {
            List<T> toReturn = new List<T>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    conn.Open();

                    using (SqlDataReader dataReader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        toReturn = ToList<T>(dataReader);
                    }
                }
            }

            // TODO: handle result being null

            return toReturn.First();
        }

        public DataSet QueryMultiple(string sql, string connectionString, CommandType commandType)
        {
            DataSet dataSet = new DataSet();
            using (var adapter = new SqlDataAdapter(sql, connectionString))
            {
                adapter.SelectCommand.CommandType = commandType;
                adapter.Fill(dataSet);
            }

            return dataSet;
        }

        // problem: because this returns an Object, can't really do anything with it
        public List<object> QueryMultiAndMap<TFirst, TSecond>(string sql, string connectionString, CommandType commandType)
        {
            var toReturn = new List<object>();

            using var connection = new SqlConnection(connectionString);
            using (var command = new SqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (reader.HasRows)
                    {
                        var first = ToList<TFirst>(reader);
                        toReturn.Add(first);
                    }

                    if (reader.NextResult())
                    {
                        var second = ToList<TSecond>(reader);
                        toReturn.Add(second);
                    }
                }
            }

            return toReturn;
        }

        // command method, return rows affected
        public int Execute(string sql, string connectionString, CommandType commandType)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sql, conn);
                conn.Open();
                command.CommandType = commandType;
                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected;
            }
        }

        // dictionary works but I don't love it - doesn't allow for param direction
        // tests needed: security, speed
        public int Execute(string sql, string connectionString, CommandType commandType, Dictionary<string, string> parameters)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sql, conn);
                conn.Open();
                command.CommandType = commandType;

                foreach (var parameter in parameters)
                {
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected;
            }
        }

    }
}
