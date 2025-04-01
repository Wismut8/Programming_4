using System.ComponentModel;
using System.Windows.Input;
using View.Model;
using View.Model.Services;

namespace View.ViewModel
{
    /// <summary>
    /// ViewModel для главного окна приложения.
    /// Реализует интерфейс <see cref="INotifyPropertyChanged"/> для уведомления об изменениях свойств.
    /// </summary>
    public class MainVM : INotifyPropertyChanged
    {
        /// <summary>
        /// Сериализатор контактов, используемый для сохранения и загрузки данных.
        /// </summary>
        private readonly ContactSerializer _contactSerializer;

        /// <summary>
        /// Текущий контакт, с которым работает ViewModel.
        /// </summary>
        private Contact _contact;
        /// <summary>
        /// Команда для сохранения контакта в файл.
        /// </summary>
        public ICommand SaveCommand { get; }
        /// <summary>
        /// Команда для загрузки контакта из файла.
        /// </summary>
        public ICommand LoadCommand { get; }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="MainVM"/>.
        /// </summary>
        public MainVM()
        {
            _contact = new Contact();
            _contactSerializer = new ContactSerializer();
            SaveCommand = new SaveCommand(_contactSerializer, _contact);
            LoadCommand = new LoadCommand(_contactSerializer, this);
        }

        /// <summary>
        /// Полное имя контакта.
        /// </summary>
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

        /// <summary>
        /// Номер телефона контакта.
        /// </summary>
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

        /// <summary>
        /// Электронная почта контакта.
        /// </summary>
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

        /// <summary>
        /// Событие, которое возникает при изменении значения свойства.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Вызывает событие <see cref="PropertyChanged"/> для уведомления об изменении свойства.
        /// </summary>
        /// <param name="propertyName">Имя изменившегося свойства.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
