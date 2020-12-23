using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using XF2MSSQL.Models;
using XF2MSSQL.Views;

namespace XF2MSSQL.ViewModels
{
    public class VendorsViewModel : BaseViewModel
    {
        public List<Vendor> Vendors { get; set; }
        public INavigation Navigation { get; set; }

        public ICommand OpenVendorCommand { get; private set; }

        public VendorsViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            OpenVendorCommand = new Command<Vendor>(vm => OpenVendor(vm));
            Vendors = new List<Vendor>();
            GetVendors();
        }

        async void OpenVendor(Vendor vendor)
        {
            await Navigation.PushAsync(new VendorPage(vendor));
        }

        private void GetVendors()
        {
            Vendors.AddRange(new List<Vendor>(){
                new Models.Vendor
                {
                    BusinessEntityID = 1,
                    AccountNumber = " ",
                    Name = "Special One",
                    CreditRating = 0,
                    ActiveFlag = true
                },
                new Models.Vendor
                {
                    BusinessEntityID = 2,
                    AccountNumber = " ",
                    Name = "Special Two",
                    CreditRating = 0,
                    ActiveFlag = true
                },
                new Models.Vendor
                {
                    BusinessEntityID = 3,
                    AccountNumber = " ",
                    Name = "Special Three",
                    CreditRating = 0,
                    ActiveFlag = false
                },
                new Models.Vendor
                {
                    BusinessEntityID = 4,
                    AccountNumber = " ",
                    Name = "Special Four",
                    CreditRating = 0,
                    ActiveFlag = false
                },
                new Models.Vendor
                {
                    BusinessEntityID = 5,
                    AccountNumber = " ",
                    Name = "Special Five",
                    CreditRating = 0,
                    ActiveFlag = true
                }
                });
        }


    }
}
