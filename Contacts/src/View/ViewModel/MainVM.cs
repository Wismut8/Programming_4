using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using View.Model;
using View.Model.Services;

namespace View.ViewModel
{
    public class MainVM : INotifyPropertyChanged
    {
        private readonly ContactSerializer _contactSerializer;
        private Contact _contact;
        public ICommand SaveCommand { get; }
        public ICommand LoadCommand { get; }

        public MainVM()
        {
            _contact = new Contact();
            _contactSerializer = new ContactSerializer();
            SaveCommand = new SaveCommand(_contactSerializer, _contact);
            LoadCommand = new LoadCommand(_contactSerializer, this);
        }

        public string FullName
        {
            get => _contact.FullName;
            set
            {
                if (_contact.FullName != value)
                {
                    _contact.FullName = value;
                    OnPropertyChanged(nameof(FullName));
                }
            }
        }

        public string PhoneNumber
        {
            get => _contact.PhoneNumber;
            set
            {
                if (_contact.PhoneNumber != value)
                {
                    _contact.PhoneNumber = value;
                    OnPropertyChanged(nameof(PhoneNumber));
                }
            }
        }

        public string Email
        {
            get => _contact.Email;
            set
            {
                if (_contact.Email != value)
                {
                    _contact.Email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
