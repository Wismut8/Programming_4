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

        /// <summary>
        /// Редактируемый контакт.
        /// </summary>
        private Contact _editingContact;

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
                    EditingContact != null &&
                    !string.IsNullOrWhiteSpace(EditingContact.FullName) &&
                    !string.IsNullOrWhiteSpace(EditingContact.PhoneNumber) &&
                    !string.IsNullOrWhiteSpace(EditingContact.Email),
                useCommandManager: true
            );

            SaveInFileCommand = new RelayCommand(
                execute: SaveInFile,
                canExecute: _ => true
            );
        }

        /// <summary>
        /// Задает и возвращает полное имя контакта.
        /// </summary>
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
        /// Задает и возвращает номер телефона контакта.
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
        /// Задает и возвращает электронную почту контакта.
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

        /// <summary>
        /// Выбранный в данный момент контакт.
        /// </summary>
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
                // Оптимизация: Удалены лишние OnPropertyChanged, оставлено только CanEditDelete
                // так как остальные свойства обновляются через IsEditing
                OnPropertyChanged(nameof(CanEditDelete));
            }
        }

        /// <summary>
        /// Контакт, находящийся в режиме редактирования.
        /// </summary>
        public Contact EditingContact
        {
            get => _editingContact;
            set
            {
                _editingContact = value;
                OnPropertyChanged(nameof(EditingContact));
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
                // Заменён вызов RefreshUI() на точечные обновления
                OnPropertyChanged(nameof(IsReadOnly)); 
                OnPropertyChanged(nameof(IsApplyVisible));
                OnPropertyChanged(nameof(CanEditDelete));
            }
        }

        // Оптимизация: Полностью удалён метод RefreshUI(), так как все обновления
        // теперь происходят напрямую в сеттерах свойств
        /* БЫЛО:
        public void RefreshUI()
        {
            OnPropertyChanged(nameof(IsReadOnly));
            OnPropertyChanged(nameof(IsApplyVisible));
            OnPropertyChanged(nameof(CanEditDelete));
        }
        */

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
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /// <summary>
        /// Метод для команды добавления контакта.
        /// </summary>
        /// <param name="parameter"></param>
        private void AddContact(object parameter)
        {
            IsEditing = true;
            EditingContact = new Contact();
            SelectedContact = null;
            // Удалён вызов RefreshUI() - IsEditing=true обновит все свойства
        }

        /// <summary>
        /// Метод для команды сохранения изменений.
        /// </summary>
        /// <param name="parameter"></param>
        private void ApplyChanges(object parameter)
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
        }

        /// <summary>
        /// Метод для команды редактирования контакта.
        /// </summary>
        /// <param name="parameter"></param>
        private void EditContact(object parameter)
        {
            EditingContact = SelectedContact.Clone();
            IsEditing = true;
            // Удалён вызов RefreshUI()
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