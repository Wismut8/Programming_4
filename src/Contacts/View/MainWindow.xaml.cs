using System.Windows;
using ViewModel;

namespace View
{
    /// <summary>
    /// Реализует логику для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Конструктор MainWindow.xaml
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainVM();
        }
    }
}
