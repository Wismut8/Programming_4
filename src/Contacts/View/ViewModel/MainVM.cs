using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using View.Model;
using View.Model.Services;


namespace View.ViewModel
{
    /// <summary>
    /// Основная ViewModel приложения, реализующая логику работы с контактами.
    /// </summary>
    public partial class MainVM : ObservableObject
    {
        /// <summary>
        /// Инициализирует сериализатор.
        /// </summary>
        private readonly ContactSerializer _serializer;

        /// <summary>
        /// Выбранный контакт.
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditCommand))]
        [NotifyCanExecuteChangedFor(nameof(RemoveCommand))]
        private Contact _selectedContact;

        /// <summary>
        /// Хранит состояние редактирования.
        /// </summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsReadOnly))]
        [NotifyPropertyChangedFor(nameof(IsApplyVisible))]
        private bool _isEditing;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ApplyChangesCommand))]
        private Contact _activeContact;

        /// <summary>
        /// Коллекция контактов для отображения.
        /// </summary>
        public ObservableCollection<Contact> Contacts { get; } = new ObservableCollection<Contact>();

        /// <summary>
        /// Конструктор основной ViewModel.
        /// </summary>
        public MainVM()
        {
            _serializer = new ContactSerializer();
            LoadContacts();
            if (_activeContact != null)
            {
                _activeContact.PropertyChanged += (s, e) => ApplyChangesCommand.NotifyCanExecuteChanged();
            }
        }

        /// <summary>
        /// Выбранный в данный момент контакт.
        /// </summary>
        partial void OnSelectedContactChanged(Contact value)
        {
            if (value != null)
            {
                if (IsEditing)
                {
                    IsEditing = false;
                }

                ActiveContact = SelectedContact.Clone();
            }
        }

        /// <summary>
        /// Флаг, указывающий на режим только для чтения.
        /// </summary>
        public bool IsReadOnly => !IsEditing;

        /// <summary>
        /// Видимость кнопки применения изменений.
        /// </summary>
        public bool IsApplyVisible => IsEditing;

        /// <summary>
        /// Флаг, указывающий на возможность редактирования/удаления.
        /// </summary>
        public bool CanEditDelete => SelectedContact != null && !IsEditing;

        private bool CanApply => IsEditing &&
            ActiveContact != null &&
            !ActiveContact.HasErrors;

        /// <summary>
        /// Загружает контакты из хранилища.
        /// </summary>
        private void LoadContacts()
        {
            foreach (var contact in _serializer.LoadContacts())
                Contacts.Add(contact);
        }

        /// <summary>
        /// Метод для команды добавления контакта.
        /// </summary>
        /// <param name="parameter"></param>
        [RelayCommand]
        private void Add()
        {
            IsEditing = true;
            ActiveContact = new Contact();
            ActiveContact.PropertyChanged += (s, e) => ApplyChangesCommand.NotifyCanExecuteChanged();
            SelectedContact = null;
        }

        /// <summary>
        /// Метод для команды сохранения изменений.
        /// </summary>
        /// <param name="parameter"></param>
        [RelayCommand(CanExecute = nameof(CanApply))]
        private void ApplyChanges()
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
        [RelayCommand(CanExecute = nameof(CanEditDelete))]
        private void Edit()
        {
            IsEditing = true;
        }

        /// <summary>
        /// Метод для команды удаления контакта.
        /// </summary>
        [RelayCommand(CanExecute =nameof(CanEditDelete))]
        private void Remove()
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
        [RelayCommand]
        private void SaveInFile()
        {
            _serializer.SaveContacts(Contacts);
        }
    }
}