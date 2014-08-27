using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SchemaBackup.Core
{
    public static class SqlMethods
    {
        public static void ExecuteCommand(string connectionStr, string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    // add parameters
                    foreach (SqlParameter parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public static IEnumerable<DataTable> ExecuteDataTableReturnCommand(string connectionStr, string sql, params SqlParameter[] parameters)
        {
            List<DataTable> datatables = new List<DataTable>();
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    // add parameters
                    foreach (SqlParameter parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }
                    SqlDataReader reader = command.ExecuteReader();
                    do
                    {
                        DataTable returnDataTable = new DataTable();
                        returnDataTable.Load(reader);
                        datatables.Add(returnDataTable);
                        if (reader.IsClosed)
                            break;
                    }
                    while (reader.NextResult());
                }
                connection.Close();
            }
            return datatables;
        }

        public static string ExecuteStringReturnCommand(string connectionStr, string sql, params SqlParameter[] parameters)
        {
            string response = string.Empty;
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    // add parameters
                    foreach (SqlParameter parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }
                    SqlDataReader sqldatareader = command.ExecuteReader();
                    response = sqldatareader.GetString(0);
                }
                connection.Close();
            }
            return response;
        }

        public static string GetDbName(string connectionStr)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                return connection.Database;
            }
        }

        public static string GetDbDataSource(string connectionStr)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                return connection.DataSource;
            }
        }
    }
}