using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace View.ViewModel
{
    public class EditCommand : ICommand
    {
        private readonly MainVM _viewModel;

        public EditCommand(MainVM viewModel)
        {
            _viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter) =>
            _viewModel.SelectedContact != null && !_viewModel.IsEditing;

        public void Execute(object parameter)
        {
            _viewModel.EditingContact = _viewModel.SelectedContact.Clone(); 
            _viewModel.IsEditing = true;
            _viewModel.RefreshUI();
        }
    }
}
