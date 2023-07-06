using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using TimeManager.Core.Models;

namespace TimeManager.WPF.Store
{
    public class MainStore
    {

        public ObservableCollection<ProcessItem> Processes { get; set; } = new();

        public ObservableCollection<ObservedProcess> ObservedProcesses { get; set; } = new();

        public ObservableCollection<ProcessObserver> ProcessObservers { get; set; } = new();

        public MainStore() 
        {
            LoadProcessList();  
        }
        public void LoadProcessList()
        {
            var processes = Process.GetProcesses();
            foreach (var process in processes)
            {
                var item = new ProcessItem
                {
                    Name = process.ProcessName,
                    Id = process.Id,
                };

                Processes.Add(item);
            }
        }

        public void OnProcessButtonClicked(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string? processName = button.CommandParameter.ToString();
            if(processName != null)
            {
                MessageBox.Show(processName);
            }
        }

        public void OnProcessClosed(ProcessObserver observer, ObservedProcess observedProcess, DateTime closedTime)
        {

        }
        public void OnProcessOpened(ProcessObserver observer, ObservedProcess observedProcess, DateTime openedTime)
        {

        }
    }
    public class ProcessItem
    {
        public string Name { get; set; } = null!;
        public int Id { get; set; }
    }
}
