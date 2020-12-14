using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using XF2MSSQL.Models;

namespace XF2MSSQL
{
    public class MySQL : MyHelper
    {
        public string GetSQLConnection()
        {
            string conn = "";

            SqlConnectionStringBuilder stringBuilder = new SqlConnectionStringBuilder();
            stringBuilder.DataSource = GetSetting("DataSource");
            stringBuilder.InitialCatalog = GetSetting("InitialCatalog");
            stringBuilder.UserID = GetSetting("UserID");
            stringBuilder.Password = GetSetting("Password");
            stringBuilder.IntegratedSecurity = false;
            stringBuilder.ApplicationName = "XF2MSSQL";
            stringBuilder.WorkstationID = "AndroidDevice";

            conn = stringBuilder.ConnectionString;

            return conn;
        }

        /// <summary>
        /// Get a single value from a SQL query (sqlDataReader.GetString(0))
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns>string value</returns>
        public string GetValue(string sqlQuery)
        {
            string ret = "";
            string connection = GetSQLConnection();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connection))
                {
                    sqlConnection.Open();

                    SqlCommand sqlCommand = new SqlCommand();

                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandText = sqlQuery;

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        ret = sqlDataReader.GetString(0);
                    }
                    sqlDataReader.Close();
                }
                return ret;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<Person> GetListPersons()
        {
            string connection = GetSQLConnection();
            List<Person> personList = new List<Person>();

            string sqlQuery = "select BusinessEntityID, FirstName, LastName from person.person where BusinessEntityID < 50";

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connection))
                {
                    sqlConnection.Open();

                    SqlCommand sqlCommand = new SqlCommand();

                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandText = sqlQuery;

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        personList.Add(new Person(sqlDataReader.GetInt32(0), sqlDataReader.GetString(1), sqlDataReader.GetString(2)));
                    }
                    sqlDataReader.Close();
                }
                return personList;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
