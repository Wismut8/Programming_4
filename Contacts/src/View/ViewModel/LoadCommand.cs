using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using View.Model.Services;
using View.Model;
using System.Windows.Input;

namespace View.ViewModel
{
    class LoadCommand : ICommand
    {
        private readonly ContactSerializer _contactSerializer;
        private readonly MainVM _viewModel;

        public LoadCommand(ContactSerializer contactSerializer, MainVM viewModel)
        {
            _contactSerializer = contactSerializer;
            _viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            try
            {
                Contact contact = _contactSerializer.LoadContact();
                _viewModel.FullName = contact.FullName;
                _viewModel.PhoneNumber = contact.PhoneNumber;
                _viewModel.Email = contact.Email;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
