using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF2MSSQL.Models;
using XF2MSSQL.ViewModels;

namespace XF2MSSQL.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VendorPage : ContentPage
    {
        public VendorPage(VendorViewModel vendor)
        {
            //Vendor vendor = new Vendor {
            //    BusinessEntityID = 1,
            //    AccountNumber = " ",
            //    Name = "Special One",
            //    CreditRating = 0,
            //    ActiveFlag = true
            //};

            BindingContext = vendor; // new VendorViewModel(vendor);

            InitializeComponent();
        }
    }
}