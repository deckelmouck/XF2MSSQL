using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XF2MSSQL
{
    public partial class MainPage : ContentPage
    {
        public string SQLreturn { get; set; }
        public int BusinessEntityID { get; set; }

        public MainPage()
        {
            InitializeComponent();
            BusinessEntityID = 1;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            SQLreturn = "try SQL query";
            myLabel.Text = SQLreturn;

            //myLabel.Text = GetSQLConnection();

            ConnectSQL(GetSQLConnection());

            BusinessEntityID++;
        }

        public void ConnectSQL(string connection)
        {
            try
            {
                using(SqlConnection sqlConnection = new SqlConnection(connection))
                {
                    sqlConnection.Open();

                    SqlCommand sqlCommand = new SqlCommand();

                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandText = string.Format("select FirstName from person.person where BusinessEntityID = {0}", BusinessEntityID.ToString());

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        myLabel.Text = sqlDataReader.GetString(0);
                    }
                    sqlDataReader.Close();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

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

        public string GetSetting(string setting)
        {
            // Get the assembly this code is executing in
            var assembly = Assembly.GetExecutingAssembly();

            // Look up the resource names and find the one that ends with settings.json
            // Your resource names will generally be prefixed with the assembly's default namespace
            // so you can short circuit this with the known full name if you wish
            var resName = assembly.GetManifestResourceNames()
                ?.FirstOrDefault(r => r.EndsWith("settings.json", StringComparison.OrdinalIgnoreCase));

            // Load the resource file
            var file = assembly.GetManifestResourceStream(resName);

            // Stream reader to read the whole file
            var sr = new StreamReader(file);

            // Read the json from the file
            var json = sr.ReadToEnd();

            // Parse out the JSON
            var j = JObject.Parse(json);

            return j.Value<string>(setting);
        }
    }
}
