using System.Windows;
using TimeManager.WPF.Store;
namespace TimeManager.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainStore Store = new();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = Store;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Store.OnProcessButtonClicked(sender, e);
        }
    }
}
