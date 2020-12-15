using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF2MSSQL.Models;

namespace XF2MSSQL
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductList : ContentPage
    {
        private ObservableCollection<Product> myproducts = new ObservableCollection<Product>();
        
        public ProductList()
        {
            InitializeComponent();

            GetProducts();
            lvProducts.ItemsSource = myproducts;

            //BindingContext = this;


        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PopAsync();
            return base.OnBackButtonPressed();
        }

        private void GetProducts()
        {
            //Product product = new Product();
            //product.Name = "test p";
            //product.ProductID = 1;
            //product.ProductNumber = "123-456FF";

            //myproducts.Add(product);

            MySQL mySQL = new MySQL();
            var productList = mySQL.GetListProducts();

            foreach (var item in productList)
            {
                myproducts.Add(item);
            }
        }

        private void OnItemSelect(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private void OnTestClicked(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            var product = menuItem.CommandParameter as Product;

            DisplayAlert("Product", product.ProductNumber, "OK");
        }
    }
}