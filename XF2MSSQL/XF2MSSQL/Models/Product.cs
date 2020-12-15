using System;
using System.Collections.Generic;
using System.Text;

namespace XF2MSSQL.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string ProductNumber { get; set; }

        public bool MakeFlag { get; set; }

        public int UpdateMakeFlag(Product product)
        {
            MySQL mySQL = new MySQL();
            string query = string.Format("UPDATE Production.Product SET MakeFlag = {0} WHERE ProductID = {1}", product.MakeFlag ? "0" : "1", product.ProductID.ToString());

            return mySQL.UpdateValue(query);
        }
    }
}
