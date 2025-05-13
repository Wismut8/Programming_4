using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using View.Model;
using View.Model.Services;

namespace View.ViewModel
{
    /// <summary>
    /// Основная ViewModel приложения, реализующая логику работы с контактами.
    /// </summary>
    public class MainVM : INotifyPropertyChanged
    {
        /// <summary>
        /// Инициализирует сериализатор.
        /// </summary>
        private readonly ContactSerializer _serializer;

        /// <summary>
        /// Выбранный контакт.
        /// </summary>
        private Contact _selectedContact;

        /// <summary>
        /// Хранит состояние редактирования.
        /// </summary>
        private bool _isEditing;

        private Contact _activeContact;

        /// <summary>
        /// Коллекция контактов для отображения.
        /// </summary>
        public ObservableCollection<Contact> Contacts { get; } = new ObservableCollection<Contact>();

        /// <summary>
        /// Команда добавления нового контакта.
        /// </summary>
        public ICommand AddCommand { get; }

        /// <summary>
        /// Команда редактирования выбранного контакта.
        /// </summary>
        public ICommand EditCommand { get; }

        /// <summary>
        /// Команда удаления выбранного контакта.
        /// </summary>
        public ICommand RemoveCommand { get; }

        /// <summary>
        /// Команда применения изменений контакта.
        /// </summary>
        public ICommand ApplyCommand { get; }

        /// <summary>
        /// Команда сохранения контактов в файл.
        /// </summary>
        public ICommand SaveInFileCommand { get; }

        /// <summary>
        /// Конструктор основной ViewModel.
        /// </summary>
        public MainVM()
        {
            _serializer = new ContactSerializer();
            LoadContacts();

            AddCommand = new RelayCommand(
                execute: AddContact,
                canExecute: _ => !IsEditing
            );

            EditCommand = new RelayCommand(
                execute: EditContact,
                canExecute: _ => SelectedContact != null && !IsEditing,
                useCommandManager: true
            );

            RemoveCommand = new RelayCommand(
                execute: RemoveContact,
                canExecute: _ => SelectedContact != null && !IsEditing,
                useCommandManager: true
            );

            ApplyCommand = new RelayCommand(
                execute: ApplyChanges,
                canExecute: _ => IsEditing &&
                    ActiveContact != null &&
                    !ActiveContact.HasErrors,
                useCommandManager: true
            );

            SaveInFileCommand = new RelayCommand(
                execute: SaveInFile,
                canExecute: _ => true
            );
        }

        /// <summary>
        /// Активный в данный момент контакт для привязки данных.
        /// </summary>
        public Contact ActiveContact
        {
            get => _activeContact;
            set
            {
                _activeContact = value;
                OnPropertyChanged(nameof(ActiveContact));
            }
        }

        /// <summary>
        /// Выбранный в данный момент контакт.
        /// </summary>
        public Contact SelectedContact
        {
            get => _selectedContact;
            set
            {
                if (_selectedContact == value) return;
                
                _selectedContact = value;

                if (value != null)
                {
                    if (IsEditing)
                    {
                        IsEditing = false;
                    }

                    ActiveContact = SelectedContact.Clone();
                }

                OnPropertyChanged(nameof(SelectedContact));
                OnPropertyChanged(nameof(ActiveContact));
                OnPropertyChanged(nameof(CanEditDelete));
            }
        }

        /// <summary>
        /// Флаг, указывающий на активный режим редактирования.
        /// </summary>
        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                if (_isEditing == value) return;
                _isEditing = value;
                OnPropertyChanged(nameof(IsEditing));
                OnPropertyChanged(nameof(IsReadOnly));
                OnPropertyChanged(nameof(IsApplyVisible));
                OnPropertyChanged(nameof(CanEditDelete));
            }
        }

        /// <summary>
        /// Флаг, указывающий на режим только для чтения.
        /// </summary>
        public bool IsReadOnly => !_isEditing;

        /// <summary>
        /// Видимость кнопки применения изменений.
        /// </summary>
        public bool IsApplyVisible => _isEditing;

        /// <summary>
        /// Флаг, указывающий на возможность редактирования/удаления.
        /// </summary>
        public bool CanEditDelete => SelectedContact != null && !_isEditing;

        /// <summary>
        /// Загружает контакты из хранилища.
        /// </summary>
        private void LoadContacts()
        {
            foreach (var contact in _serializer.LoadContacts())
                Contacts.Add(contact);
        }

        /// <summary>
        /// Событие, возникающее при изменении значения свойства.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Вызывает событие PropertyChanged при изменении свойства.
        /// </summary>
        /// <param name="propertyName">Имя изменившегося свойства</param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Метод для команды добавления контакта.
        /// </summary>
        /// <param name="parameter"></param>
        private void AddContact(object parameter)
        {
            IsEditing = true;
            ActiveContact = new Contact();
            SelectedContact = null;
        }

        /// <summary>
        /// Метод для команды сохранения изменений.
        /// </summary>
        /// <param name="parameter"></param>
        private void ApplyChanges(object parameter)
        {
            if (SelectedContact != null)
            {
                SelectedContact.FullName = ActiveContact.FullName;
                SelectedContact.PhoneNumber = ActiveContact.PhoneNumber;
                SelectedContact.Email = ActiveContact.Email;
            }
            else
            {
                var newContact = new Contact(
                    ActiveContact.FullName,
                    ActiveContact.PhoneNumber,
                    ActiveContact.Email);
                Contacts.Add(newContact);
                SelectedContact = newContact;
            }
            IsEditing = false;
            OnPropertyChanged(nameof(ActiveContact));
        }

        /// <summary>
        /// Метод для команды редактирования контакта.
        /// </summary>
        /// <param name="parameter"></param>
        private void EditContact(object parameter)
        {
            IsEditing = true;
        }

        /// <summary>
        /// Метод для команды удаления контакта.
        /// </summary>
        /// <param name="parameter"></param>
        private void RemoveContact(object parameter)
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
        }

        /// <summary>
        /// Метод для команды сохранения и загрузки в файл.
        /// </summary>
        /// <param name="parameter"></param>
        private void SaveInFile(object parameter)
        {
            _serializer.SaveContacts(Contacts);
        }
    }
}