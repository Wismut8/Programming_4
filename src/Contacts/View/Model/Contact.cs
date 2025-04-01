using System;

namespace View.Model
{
    /// <summary>
    /// Контакт.
    /// </summary>
    public class Contact
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
        public string FullName { get; set; }

        /// <summary>
        /// Возвращает и задает номер телефона.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Возвращает и задает электронную почту.
        /// </summary>
        public string Email { get; set; }

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

    }
}
