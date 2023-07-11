using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;
using TimeManager.WPF.Store;
using TimeManager.WPF.StartUpManager;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Reflection;
using System.Diagnostics;

namespace TimeManager.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const string AppName = "TimeManager";
        public const string PathToIcon = "";
        private ICollectionView ProcessesFiltred;
        MainStore Store = new();
        private DispatcherTimer timerForProcesses = new();
        private DispatcherTimer SaveChangesTimer = new();
        private System.Windows.Forms.NotifyIcon notifyIcon = new();
        public MainWindow()
        {
            InitializeComponent();
            SetUpNotifyIcon(notifyIcon);
            Hide();
            ShowInTaskbar = false;
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

        public void SetUpNotifyIcon(System.Windows.Forms.NotifyIcon notifyIcon)
        {
            void ShowWindow()
            {
                Show();
                WindowState = WindowState.Normal;
                ShowInTaskbar = true;
                notifyIcon.Visible = false;
            }
            notifyIcon.Icon = GetCurrentIcon();
            notifyIcon.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            notifyIcon.ContextMenuStrip.Items.Add("Open", null, (_,_) => { ShowWindow(); });
            notifyIcon.ContextMenuStrip.Items.Add("Exit", null, (_, _) => { Close(); });
            notifyIcon.Visible = true;
        }

        public Icon GetCurrentIcon()
        {
            string? pathToFile = Process.GetCurrentProcess()?.MainModule?.FileName;
            if (pathToFile == null)
            {
                MessageBox.Show("Error has ocured!");
                Close();
                throw new Exception("No process");
            }
            var ico = System.Drawing.Icon.ExtractAssociatedIcon(pathToFile);
            if(ico == null)
            {
                throw new Exception("No ico was set");
            }
            return ico;
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
            /*Application.Current.Activated += (_, _) =>
            {
                if(WindowState == WindowState.Minimized)
                {
                    Hide();
                    notifyIcon.Visible = true;
                    ShowInTaskbar = false;
                }
            };*/
            
            IsVisibleChanged += (_, _) =>
            {
                if (IsVisible)
                {
                    ShowInTaskbar = true;
                    notifyIcon.Visible = false;
                    timerForProcesses.Start();
                }
                else
                {
                    ShowInTaskbar = false;
                    notifyIcon.Visible = true;
                    timerForProcesses.Stop();
                }
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

        private void OnWindowStateChanged(object sender, EventArgs e)
        {
            if(WindowState == WindowState.Minimized)
            {
                ShowInTaskbar = false;
                notifyIcon.Visible = true;
                return;
            }
            if(WindowState == WindowState.Normal)
            {
                ShowInTaskbar = true;
                notifyIcon.Visible = false;
                return;
            }
        }
    }
}
