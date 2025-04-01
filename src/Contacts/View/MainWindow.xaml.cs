using System.Windows;
using View.ViewModel;

namespace View
{
    /// <summary>
    /// Реализует логику для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainVM();
        }
    }
}
