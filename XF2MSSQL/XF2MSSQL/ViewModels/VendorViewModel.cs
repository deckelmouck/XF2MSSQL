using System;
using System.Collections.Generic;
using System.Text;
using XF2MSSQL.Models;

namespace XF2MSSQL.ViewModels
{
    public class VendorViewModel : BaseViewModel
    {
        public int BusinessEntityID { get; set; }
        public string AccountNumber { get; set; }
        public string Name { get; set; }
        public int CreditRating { get; set; }
        public bool ActiveFlag { get; set; }

        public VendorViewModel(Vendor vendor)
        {
            BusinessEntityID = vendor.BusinessEntityID;
            AccountNumber = vendor.AccountNumber;
            Name = vendor.Name;
            CreditRating = vendor.CreditRating;
            ActiveFlag = vendor.ActiveFlag;
        }
    }
}
