using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using View.Model;
using View.Model.Services;

namespace View.ViewModel
{
    /// <summary>
    /// Сохранение контакта.
    /// </summary>
    class SaveCommand : ICommand
    {
        private readonly ContactSerializer _contactSerializer;
        private readonly Contact _contact;

        public SaveCommand(ContactSerializer contactSerializer, Contact contact)
        {
            _contactSerializer = contactSerializer;
            _contact = contact;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) { return true; }

        public void Execute(object parameter)
        {
            _contactSerializer.SaveContact(_contact);
        }
    }
}
