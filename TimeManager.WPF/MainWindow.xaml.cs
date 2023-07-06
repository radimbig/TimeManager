using System.Windows;
using System.Windows.Controls;
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
            Button button = (Button)sender;
            string? processName = button.CommandParameter.ToString();
            if (processName == null)
            {
                return;
            }
            Store.AddObservedProcess(processName);
        }

        private void RemoveObserving(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string? processName = button.CommandParameter.ToString();
            if (processName == null)
            {
                return;
            }
            Store.RemoveObservedProcess(processName);
        }
    }
}
