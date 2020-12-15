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

            string sqlQuery = string.Format("SELECT FirstName FROM person.person WHERE BusinessEntityID = {0}", BusinessEntityID.ToString());

            MySQL mySql = new MySQL();
            myLabel.Text = mySql.GetValue(sqlQuery);

            BusinessEntityID++;
        }

        async void openPersonList(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PersonList());
        }

        async void openProducts(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProductList());
        }

    }
}
