using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;
using TimeManager.WPF.Store;
using TimeManager.WPF.StartUpManager;
namespace TimeManager.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const string AppName = "TimeManager";
        private ICollectionView ProcessesFiltred;
        MainStore Store = new();
        private DispatcherTimer timerForProcesses = new();
        private DispatcherTimer SaveChangesTimer = new();
        private System.Windows.Forms.NotifyIcon notifyIcon = new();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = Store;
            // Datacontexts
            ProcessesFiltred = CollectionViewSource.GetDefaultView(Store.Processes);
            ProcessesListView.ItemsSource = ProcessesFiltred;
            ObservedProcessesListView.ItemsSource = Store.ObservedProcesses;
            // StartUpCheckBox
            IsStartUpEnabledCheckBox.IsChecked = StartUpManager.StartUpManager.IsStartUpEnabled(AppName);
            // timer for processes
            SetAutoProcessesLoading(timerForProcesses);
            timerForProcesses.Start();
            // timer for saving changes every 60 seconds
            SetAutoSaving(SaveChangesTimer);
            SaveChangesTimer.Start();
            // events
            SetOnHideCloseEvents();
        }

        public void SetAutoProcessesLoading(DispatcherTimer timer, int delay = 5)
        {
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += (_, _) =>
            {
                Store.LoadProcessList();
            };
        }

        public void SetAutoSaving(DispatcherTimer timer, int deley = 60)
        {
            timer.Interval = TimeSpan.FromSeconds(deley);
            timer.Tick += (_, _) =>
            {
                Store.SaveChanges();
            };
        }



        public void SetOnHideCloseEvents()
        {
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

        private void OnStartUpCheckBoxChanged(object sender, RoutedEventArgs e)
        {
            if(sender is not CheckBox checkBox)
            {
                return;
            }
            if(checkBox.IsChecked is null)
            {
                return;
            }
            if(checkBox.IsChecked == true)
            {
                StartUpManager.StartUpManager.SetStartUp(AppName, true);
                return;
            }
            StartUpManager.StartUpManager.SetStartUp(AppName, false);
        }
    }
}
