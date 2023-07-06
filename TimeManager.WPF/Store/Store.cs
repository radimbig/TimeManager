using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
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

        public void AddObservedProcess(string processName)
        {
            var tempProcess = new ObservedProcess(processName);
            tempProcess.OpenedAt = SetStartUpTimeOrReturnDefaultValue(processName);
            ObservedProcesses.Add(tempProcess);
            ProcessObservers.Add(new ProcessObserver(tempProcess, OnProcessClosed, OnProcessOpened));
            _ = ProcessObservers.Last().StartObservingAsync();
        }
        public void RemoveObservedProcess(string processName)
        {
            if(!ObservedProcesses.Any(x=>x.Name == processName))
            {
                return;
            }
            var observer = ProcessObservers.FirstOrDefault(x => x.TargetProcess.Name == processName);
            if(observer == null)
            {
                return;
            }
            observer.StopObserving();
            ProcessObservers.Remove(observer);

            var targetProcess = ObservedProcesses.FirstOrDefault(x => x.Name == processName);
            if(targetProcess == null) 
            {
                throw new Exception("No any process with what name");
            }
            ObservedProcesses.Remove(targetProcess);
        }
        public DateTime SetStartUpTimeOrReturnDefaultValue(string processName)
        {
            var processes = Process.GetProcessesByName(processName);
            if(processes.Length > 0)
            {
                try
                {
                    var startUpTime = processes.First().StartTime;
                    return startUpTime;
                }
                catch { }
            }
            return new DateTime();
        }

        public void OnProcessClosed(ProcessObserver observer, ObservedProcess observedProcess, DateTime closedTime)
        {
            observedProcess.ClosedAt = closedTime;
            MessageBox.Show($"observer notices that {observedProcess.Name} was closed at {closedTime}");
        }
        public void OnProcessOpened(ProcessObserver observer, ObservedProcess observedProcess, DateTime openedTime)
        {
            
            observedProcess.OpenedAt = openedTime;
            MessageBox.Show($"observer notices that {observedProcess.Name} was opened at {openedTime}");
        }
    }
    public class ProcessItem
    {
        public string Name { get; set; } = null!;
        public int Id { get; set; }
    }
}
