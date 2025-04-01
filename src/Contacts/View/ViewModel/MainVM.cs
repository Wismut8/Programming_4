using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using View.Model;
using View.Model.Services;

namespace View.ViewModel
{
    public class MainVM : INotifyPropertyChanged
    {
        private readonly ContactSerializer _serializer;
        private Contact _selectedContact;
        private bool _isEditing;
        private Contact _editingContact;

        public ObservableCollection<Contact> Contacts { get; } = new ObservableCollection<Contact>();
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand RemoveCommand { get; }
        public ICommand ApplyCommand { get; }

        public MainVM()
        {
            _serializer = new ContactSerializer();
            LoadContacts();

            AddCommand = new AddCommand(this);
            EditCommand = new EditCommand(this);
            RemoveCommand = new RemoveCommand(this);
            ApplyCommand = new ApplyCommand(this);
        }

        public string FullName
        {
            get => IsEditing ? EditingContact?.FullName : SelectedContact?.FullName;
            set
            {
                if (IsEditing)
                {
                    if (EditingContact != null && EditingContact.FullName != value)
                    {
                        EditingContact.FullName = value;
                        OnPropertyChanged(nameof(FullName));
                        CommandManager.InvalidateRequerySuggested();
                    }
                }
                else if (SelectedContact != null && SelectedContact.FullName != value)
                {
                    SelectedContact.FullName = value;
                    OnPropertyChanged(nameof(FullName));
                }
            }
        }

        /// <summary>
        /// Задает и возвращает номер контакта.
        /// </summary>
        public string PhoneNumber
        {
            get => IsEditing ? EditingContact?.PhoneNumber : SelectedContact?.PhoneNumber;
            set
            {
                if (IsEditing)
                {
                    if (EditingContact != null && EditingContact.PhoneNumber != value)
                    {
                        EditingContact.PhoneNumber = value;
                        OnPropertyChanged(nameof(PhoneNumber));
                        CommandManager.InvalidateRequerySuggested();
                    }
                }
                else if (SelectedContact != null && SelectedContact.PhoneNumber != value)
                {
                    SelectedContact.PhoneNumber = value;
                    OnPropertyChanged(nameof(PhoneNumber));
                }
            }
        }

        /// <summary>
        /// Задает и возвращает почту контакта.
        /// </summary>
        public string Email
        {
            get => IsEditing ? EditingContact?.Email : SelectedContact?.Email;
            set
            {
                if (IsEditing)
                {
                    if (EditingContact != null && EditingContact.Email != value)
                    {
                        EditingContact.Email = value;
                        OnPropertyChanged(nameof(Email));
                        CommandManager.InvalidateRequerySuggested();
                    }
                }
                else if (SelectedContact != null && SelectedContact.Email != value)
                {
                    SelectedContact.Email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        public Contact SelectedContact
        {
            get => _selectedContact;
            set
            {
                if (_selectedContact == value) return;

                if (IsEditing && value != null)
                {
                    IsEditing = false;
                }

                _selectedContact = value;
                OnPropertyChanged(nameof(SelectedContact));
                OnPropertyChanged(nameof(FullName));
                OnPropertyChanged(nameof(PhoneNumber));
                OnPropertyChanged(nameof(Email));
                OnPropertyChanged(nameof(IsReadOnly));
                OnPropertyChanged(nameof(CanEditDelete));
            }
        }
        public Contact EditingContact
        {
            get => _editingContact;
            set
            {
                _editingContact = value;
                OnPropertyChanged(nameof(EditingContact));
                OnPropertyChanged(nameof(FullName));
                OnPropertyChanged(nameof(PhoneNumber));
                OnPropertyChanged(nameof(Email));
            }
        }

        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                _isEditing = value;
                OnPropertyChanged(nameof(IsEditing));
                OnPropertyChanged(nameof(IsReadOnly));
                OnPropertyChanged(nameof(IsApplyVisible));
                OnPropertyChanged(nameof(CanEditDelete));
            }
        }

        public bool IsReadOnly => !_isEditing;
        public Visibility IsApplyVisible => _isEditing ? Visibility.Visible : Visibility.Collapsed;
        public bool CanEditDelete => SelectedContact != null && !_isEditing;

        public void ApplyChanges()
        {
            if (EditingContact != null)
            {
                if (SelectedContact != null)
                {
                    SelectedContact.FullName = EditingContact.FullName;
                    SelectedContact.PhoneNumber = EditingContact.PhoneNumber;
                    SelectedContact.Email = EditingContact.Email;
                }
                else
                {
                    var newContact = new Contact(
                        EditingContact.FullName,
                        EditingContact.PhoneNumber,
                        EditingContact.Email);
                    Contacts.Add(newContact);
                    SelectedContact = newContact;
                }
            }

            EditingContact = null;
            IsEditing = false;
            SaveContacts();
        }

        public void RemoveContact()
        {
            int index = Contacts.IndexOf(SelectedContact);
            Contacts.Remove(SelectedContact);

            if (Contacts.Count > 0)
            {
                if (index >= Contacts.Count)
                {
                    index = Contacts.Count - 1;
                }
                SelectedContact = Contacts[index];
            }

            SaveContacts();
        }

        private void LoadContacts()
        {
            foreach (var contact in _serializer.LoadContacts())
                Contacts.Add(contact);
        }

        private void SaveContacts()
        {
            _serializer.SaveContacts(Contacts);
        }

        public void RefreshUI()
        {
            OnPropertyChanged(nameof(IsReadOnly));
            OnPropertyChanged(nameof(IsApplyVisible));
            OnPropertyChanged(nameof(CanEditDelete));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}