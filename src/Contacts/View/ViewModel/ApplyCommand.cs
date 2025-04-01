using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace View.ViewModel
{
    public class ApplyCommand : ICommand
    {
        private readonly MainVM _viewModel;

        public ApplyCommand(MainVM viewModel)
        {
            _viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter) =>
            _viewModel.IsEditing &&
            _viewModel.EditingContact != null &&
            !string.IsNullOrWhiteSpace(_viewModel.EditingContact.FullName);

        public void Execute(object parameter)
        {
            _viewModel.ApplyChanges();
        }
    }
}
