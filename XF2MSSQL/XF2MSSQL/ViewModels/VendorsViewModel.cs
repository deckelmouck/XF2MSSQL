using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using XF2MSSQL.Views;

namespace XF2MSSQL.ViewModels
{
    public class VendorsViewModel : BaseViewModel
    {
        public List<VendorViewModel> Vendors { get; set; }
        public INavigation Navigation { get; set; }

        public ICommand OpenVendorCommand { get; private set; }

        public VendorsViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            OpenVendorCommand = new Command<VendorViewModel>(vm => OpenVendor(vm));
            Vendors = new List<VendorViewModel>();
            GetVendors();
        }

        async void OpenVendor(VendorViewModel vendor)
        {
            await Navigation.PushAsync(new VendorPage(vendor));
        }

        private void GetVendors()
        {
            Vendors.AddRange(new List<VendorViewModel>(){
                new VendorViewModel(new Models.Vendor
                {
                    BusinessEntityID = 1,
                    AccountNumber = " ",
                    Name = "Special One",
                    CreditRating = 0,
                    ActiveFlag = true
                }),
                new VendorViewModel(new Models.Vendor
                {
                    BusinessEntityID = 1,
                    AccountNumber = " ",
                    Name = "Special Two",
                    CreditRating = 0,
                    ActiveFlag = true
                }),
                new VendorViewModel(new Models.Vendor
                {
                    BusinessEntityID = 1,
                    AccountNumber = " ",
                    Name = "Special Three",
                    CreditRating = 0,
                    ActiveFlag = true
                }),
                new VendorViewModel(new Models.Vendor
                {
                    BusinessEntityID = 1,
                    AccountNumber = " ",
                    Name = "Special Four",
                    CreditRating = 0,
                    ActiveFlag = true
                }),
                new VendorViewModel(new Models.Vendor
                {
                    BusinessEntityID = 1,
                    AccountNumber = " ",
                    Name = "Special Five",
                    CreditRating = 0,
                    ActiveFlag = true
                })
                });
        }


    }
}
