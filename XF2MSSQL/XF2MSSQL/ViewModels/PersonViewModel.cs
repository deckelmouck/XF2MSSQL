using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XF2MSSQL.Models;

namespace XF2MSSQL.ViewModels
{
    public class PersonViewModel : BaseViewModel
    {
        public string Name { get; set; }

        private bool _isFavorite;

        public bool IsFavorite
        {
            get { return _isFavorite; }
            set 
            {
                SetValue(ref _isFavorite, value);
                OnPropertyChanged(nameof(Color));
            }
        }


        public Color Color 
        { 
            get { return IsFavorite ? Color.Green : Color.Black; } 
        }

        public PersonViewModel(Person person)
        {
            Name = person.Name;
        }
    }
}
