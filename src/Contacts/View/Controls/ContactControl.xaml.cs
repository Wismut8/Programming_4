using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using View.Model;

namespace View.Controls
{
    /// <summary>
    /// Логика взаимодействия для ContactControl.xaml
    /// </summary>
    public partial class ContactControl : UserControl
    {
        public ContactControl()
        {
            InitializeComponent();
        }
        public static readonly DependencyProperty CurrentContactProperty =
    DependencyProperty.Register("CurrentContact", typeof(Contact), typeof(ContactControl),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public Contact CurrentContact
        {
            get => (Contact)GetValue(CurrentContactProperty);
            set => SetValue(CurrentContactProperty, value);
        }
    }
}
