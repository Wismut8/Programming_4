using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Model
{
    /// <summary>
    /// Контакт.
    /// </summary>
    public class Contact : INotifyPropertyChanged, IDataErrorInfo
    {
        private static int _idCounter;

        private int _id;

        /// <summary>
        /// Хранит полное имя (Фамилия Имя Отчество).
        /// </summary>
        private string _fullName;

        /// <summary>
        /// Хранит номер телефона.
        /// </summary>
        private string _phoneNumber;

        /// <summary>
        /// Хранит электронную почту.
        /// </summary>
        private string _email;

        /// <summary>
        /// Возвращает и задает полное имя покупателя.
        /// </summary>
        public string FullName
        {
            get => _fullName;
            set
            {
                if (_fullName != value)
                {
                    _fullName = value;
                    OnPropertyChanged(nameof(FullName));
                }
            }
        }

        /// <summary>
        /// Возвращает и задает номер телефона.
        /// </summary>
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (_phoneNumber != value)
                {
                    _phoneNumber = value;
                    OnPropertyChanged(nameof(PhoneNumber));
                }
            }
        }

        /// <summary>
        /// Возвращает и задает электронную почту.
        /// </summary>
        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        /// <summary>
        /// Создает экземпляр <see cref="Contact"/>
        /// </summary>
        /// <param name="fullName">Полное имя.</param>
        /// <param name="phoneNumber">Телефонный номер.</param>
        /// <param name="email">Электронная почта.</param>
        public Contact(string fullName, string phoneNumber, string email)
        {
            _id = _idCounter++;
            FullName = fullName;
            PhoneNumber = phoneNumber;
            Email = email;
        }

        /// <summary>
        /// Создает экземпляр <see cref="Contact"/>
        /// </summary>
        public Contact() {}

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

        /// <summary>
        /// Создает копию объекта типа <see cref="Contact"/>
        /// </summary>
        /// <returns>Копия объекта <see cref="Contact"/></returns>
        public Contact Clone()
        {
            return new Contact
            {
                _id = _idCounter++,
                FullName = this.FullName,
                PhoneNumber = this.PhoneNumber,
                Email = this.Email
            };
        }

        /// <summary>
        /// Индексатор для валидации свойств объекта по имени свойства.
        /// </summary>
        /// <param name="columnName">Имя свойства для валидации.</param>
        /// <returns> Строка с сообщением об ошибке, если валидация не прошла.</returns>
        /// Выполняет следующие проверки:
        /// Для <see cref="FullName"/>: длина не более 100 символов.
        /// Для <see cref="PhoneNumber"/>: длина не более 100 символов,
        /// содержит только допустимые символы (цифры, +, -, (, ), пробелы).
        /// Для <see cref="Email"/>: длина не более 100 символов, содержит '@'.
        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;
                switch (columnName)
                {
                    case nameof(FullName):
                        if (FullName != null)
                        {
                            if (FullName.Length > 100)
                            {
                                error = "Полное имя не должно превышать 100 символов";
                            }
                        }
                        break;

                    case nameof(PhoneNumber):
                        if (PhoneNumber != null)
                        {
                            if (PhoneNumber.Length > 100)
                            {
                                error = "Номер телефона не должен превышать 100 символов";
                            }
                            else if (!Regex.IsMatch(PhoneNumber, @"^[\+\-\(\)\d\s]+$"))
                            {
                                error = "Номер телефона может содержать только цифры или символы +-() и пробелы";
                            }
                        }
                        break;

                    case nameof(Email):
                        if (Email != null) {
                            if (Email.Length > 100)
                            {
                                error = "Email не должен превышать 100 символов";
                            }
                            else if (!Email.Contains("@"))
                            {
                                error = "Email должен содержать символ @";
                            }
                        }
                        break;
                }
                return error;
            }
        }

        /// <summary>
        /// Получает сообщение об ошибке для всего объекта.
        ///Всегда возвращает null, т.к. в данной реализации проверяются только 
        /// отдельные свойства, а не весь объект в целом.
        /// </summary>
        public string Error => null;

        /// <summary>
        /// Проверяет, есть ли ошибки валидации
        /// </summary>
        public bool HasErrors
        {
            get
            {
                return !string.IsNullOrEmpty(this[nameof(FullName)]) ||
                       !string.IsNullOrEmpty(this[nameof(PhoneNumber)]) ||
                       !string.IsNullOrEmpty(this[nameof(Email)]);
            }
        }
    }
}
