using System;
using System.Windows.Input;
using View.Model;
using View.Model.Services;

namespace View.ViewModel
{
    /// <summary>
    /// Команда для сохранения контакта в файл.
    /// Реализует интерфейс <see cref="ICommand"/>.
    /// </summary>
    class SaveCommand : ICommand
    {
        /// <summary>
        /// Сериализатор контактов, используемый для сохранения данных.
        /// </summary>
        private readonly ContactSerializer _contactSerializer;

        /// <summary>
        /// Контакт, который будет сохранен в файл.
        /// </summary>
        private readonly Contact _contact;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SaveCommand"/>.
        /// </summary>
        /// <param name="contactSerializer">Сериализатор контактов.</param>
        /// <param name="contact">Контакт, который будет сохранен.</param>
        public SaveCommand(ContactSerializer contactSerializer, Contact contact)
        {
            _contactSerializer = contactSerializer;
            _contact = contact;
        }

        /// <summary>
        /// Событие, которое возникает при изменении возможности выполнения команды.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Определяет, может ли команда быть выполнена.
        /// </summary>
        /// <param name="parameter">Параметр команды.</param>
        /// <returns>Всегда возвращает <see langword="true"/>, так как команда может быть выполнена в любой момент.</returns>
        public bool CanExecute(object parameter) { return true; }

        /// <summary>
        /// Выполняет сохранение контакта в файл.
        /// </summary>
        /// <param name="parameter">Параметр команды.</param>
        public void Execute(object parameter)
        {
            _contactSerializer.SaveContact(_contact);
        }
    }
}
