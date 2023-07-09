using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;
using TimeManager.WPF.Store;

namespace TimeManager.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ICollectionView ProcessesFiltred;
        MainStore Store = new();
        private DispatcherTimer timerForProcesses = new();
        private DispatcherTimer SaveChangesTimer = new();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = Store;
            // Datacontexts
            ProcessesFiltred = CollectionViewSource.GetDefaultView(Store.Processes);
            ProcessesListView.ItemsSource = ProcessesFiltred;
            ObservedProcessesListView.ItemsSource = Store.ObservedProcesses;
            // timer for processes
            timerForProcesses.Interval = TimeSpan.FromSeconds(5);
            timerForProcesses.Tick += (_, _) =>
            {
                Store.LoadProcessList();
            };
            timerForProcesses.Start();
            // timer for saving changes every 60 seconds
            SaveChangesTimer.Interval = TimeSpan.FromSeconds(60);
            SaveChangesTimer.Tick += (_, _) =>
            {
                Store.SaveChanges();
            };
            SaveChangesTimer.Start();
            // events
            IsVisibleChanged += (_, _) =>
            {
                if (IsVisible)
                    timerForProcesses.Start();
                else
                    timerForProcesses.Stop();
            };
            Closed += (_, _) =>
            {
                Store.CloseUnclosed();
                Store.StopObservingAll();
                Store.SaveChanges();
            };
            AppDomain.CurrentDomain.ProcessExit += (_, _) =>
            {
                Store.CloseUnclosed();
                Store.StopObservingAll();
                Store.SaveChanges();
            };
        }


        private void OnButtonAddObserver(object sender, RoutedEventArgs e)
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

        private void SearchQueryChanged(object sender, TextChangedEventArgs e)
        {
            string? searchTerm = SearchQuery.Text.Trim();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                ProcessesFiltred.Filter = (obj) =>
                {
                    if (obj is ProcessItem item)
                    {
                        return item.Name.ToLower().Contains(searchTerm.ToLower());
                    }
                    return false;
                };
                return;
            }
            ProcessesFiltred.Filter = null; // Remove the filter
        }
    }
}
