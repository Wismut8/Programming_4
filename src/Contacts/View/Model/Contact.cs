using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;

namespace View.Model
{
    /// <summary>
    /// Контакт.
    /// </summary>
    public class Contact : INotifyPropertyChanged
    {
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
    }
}
