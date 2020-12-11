using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XF2MSSQL
{
    public partial class MainPage : ContentPage
    {
        public string SQLreturn { get; set; }

        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            SQLreturn = "try SQL query";
            myLabel.Text = SQLreturn;

        }

        public string GetSQLConnection()
        {
            string conn = "";



            return conn;
        }
    }
}
