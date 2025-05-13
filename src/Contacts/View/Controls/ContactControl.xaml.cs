using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace View.Controls
{
    /// <summary>
    /// Пользовательский элемент управления для отображения и редактирования контакта.
    /// </summary>
    public partial class ContactControl : UserControl
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ContactControl"/>.
        /// </summary>
        public ContactControl()
        {
            InitializeComponent();
            PhoneNumberTextBox.PreviewTextInput += PhoneNumberTextBox_PreviewTextInput;
            DataObject.AddPastingHandler(PhoneNumberTextBox, PhoneNumberTextBox_Pasting);
        }

        /// <summary>
        /// Обработчик события ввода текста в поле номера телефона.
        /// Разрешает ввод только цифр и символов +-() и пробелов.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PhoneNumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex(@"^[\d\-\+\(\)\s]+$");
            e.Handled = !regex.IsMatch(e.Text);
        }

        /// <summary>
        /// Обработчик события вставки текста в поле номера телефона.
        /// Проверяет вставляемый текст на соответствие допустимым символам.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PhoneNumberTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                var text = (string)e.DataObject.GetData(typeof(string));
                var regex = new Regex(@"^[\d\-\+\(\)\s]+$");

                if (!regex.IsMatch(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        /// <summary>
        /// DependencyProperty для свойства IsReadOnly.
        /// </summary>
        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(ContactControl),
            new PropertyMetadata(true));

        /// <summary>
        /// Флажок доступа элемента управления только для чтения.
        /// </summary>
        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }

    }
}
