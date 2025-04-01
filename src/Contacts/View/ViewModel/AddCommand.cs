using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using View.Model;

namespace View.ViewModel
{
    public class AddCommand : ICommand
    {
        private readonly MainVM _viewModel;

        public AddCommand(MainVM viewModel)
        {
            _viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter) => !_viewModel.IsEditing;

        public void Execute(object parameter)
        {
            _viewModel.IsEditing = true;
            _viewModel.EditingContact = new Contact();
            _viewModel.SelectedContact = null;
            _viewModel.RefreshUI();
        }
    }
}
