using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF2MSSQL.Models;
using XF2MSSQL.ViewModels;

namespace XF2MSSQL
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PersonList : ContentPage
    {
        //List<Person> myPersonList = new List<Person>();

        public PersonList()
        {
            BindingContext = new PersonsViewModel();
            ViewModel = new PersonsViewModel();
            InitializeComponent();
            //GetList();
            //PersonListView.ItemsSource = myPersonList;
        }

        public PersonsViewModel ViewModel
        {
            get { return BindingContext as PersonsViewModel; }
            set { BindingContext = value; }
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PopAsync();
            return base.OnBackButtonPressed();
        }

        private void OnPersonSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ViewModel.SelectedPersonCommand.Execute(e.SelectedItem);
        }
    }
}