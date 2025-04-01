using System;
using View.Model.Services;
using View.Model;
using System.Windows.Input;

namespace View.ViewModel
{
    /// <summary>
    /// Команда для загрузки контакта из файла.
    /// Реализует интерфейс <see cref="ICommand"/>.
    /// </summary>
    class LoadCommand : ICommand
    {
        /// <summary>
        /// Сериализатор контактов, используемый для загрузки данных.
        /// </summary>
        private readonly ContactSerializer _contactSerializer;

        /// <summary>
        /// ViewModel, в которую будут загружены данные контакта.
        /// </summary>
        private readonly MainVM _viewModel;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="LoadCommand"/>.
        /// </summary>
        /// <param name="contactSerializer">Сериализатор контактов.</param>
        /// <param name="viewModel">ViewModel, в которую будут загружены данные.</param>
        public LoadCommand(ContactSerializer contactSerializer, MainVM viewModel)
        {
            _contactSerializer = contactSerializer;
            _viewModel = viewModel;
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
        public bool CanExecute(object parameter) => true;

        /// <summary>
        /// Выполняет загрузку контакта из файла и обновляет ViewModel.
        /// </summary>
        /// <param name="parameter">Параметр команды.</param>
        public void Execute(object parameter)
        {
            Contact contact = _contactSerializer.LoadContact();
            _viewModel.FullName = contact.FullName;
            _viewModel.PhoneNumber = contact.PhoneNumber;
            _viewModel.Email = contact.Email;
        }
    }
}
