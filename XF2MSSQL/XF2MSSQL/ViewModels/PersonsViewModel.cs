using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace XF2MSSQL.ViewModels
{
    public class PersonsViewModel : BaseViewModel
    {
        public ObservableCollection<PersonViewModel> Persons { get; private set; }

        private PersonViewModel _selectedPerson;

        public PersonViewModel SelectedPerson
        {
            get { return _selectedPerson; }
            set { SetValue(ref _selectedPerson, value); }
        }

        public ICommand SelectedPersonCommand { get; private set; }

        public PersonsViewModel()
        {
            SelectedPersonCommand = new Command<PersonViewModel>(vm => SelectPerson(vm));
            Persons = new ObservableCollection<PersonViewModel>();
            GetList();
        }

        private void SelectPerson(PersonViewModel person)
        {
            if (person == null)
                return;

            person.IsFavorite = !person.IsFavorite;

            SelectedPerson = null; 
        }

        public void GetList()
        {
            MySQL mySql = new MySQL();
            var personlist =  mySql.GetListPersons();

            foreach (var item in personlist)
            {
                Persons.Add(new PersonViewModel(item));
            }
        }

    }
}
