using System;
using System.Collections.Generic;
using System.Text;

namespace XF2MSSQL.Models
{
    public class Vendor
    {
        public int BusinessEntityID { get; set; }
        public string AccountNumber { get; set; }
        public string Name { get; set; }
        public int CreditRating { get; set; }
        public bool ActiveFlag { get; set; }

    }
}
