using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Model;
using Model.Services;


namespace ViewModel
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

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditCommand))]
        [NotifyCanExecuteChangedFor(nameof(RemoveCommand))]
        private Contact _selectedContact;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsReadOnly))]
        [NotifyPropertyChangedFor(nameof(IsApplyVisible))]
        [NotifyCanExecuteChangedFor(nameof(EditCommand))]
        [NotifyCanExecuteChangedFor(nameof(RemoveCommand))]
        [NotifyCanExecuteChangedFor(nameof(ApplyChangesCommand))]
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
        }

        partial void OnSelectedContactChanged(Contact value)
        {
            if (value != null)
            {
                if (IsEditing)
                {
                    IsEditing = false;
                }

                ActiveContact = SelectedContact.Clone();
                if (ActiveContact != null)
                {
                    ActiveContact.PropertyChanged += (s, e) => ApplyChangesCommand.NotifyCanExecuteChanged();
                }
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
        }

        [RelayCommand(CanExecute = nameof(CanEditDelete))]
        private void Edit()
        {
            IsEditing = true;
        }

        [RelayCommand(CanExecute = nameof(CanEditDelete))]
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