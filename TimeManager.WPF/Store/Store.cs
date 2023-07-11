using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using TimeManager.Core.Models;
using TimeManager.Database.Commands;
using TimeManager.Database.Queries;
using TimeManager.WPF.ViewModels;
namespace TimeManager.WPF.Store
{
    public class MainStore
    {
        public ObservableCollection<ProcessItem> Processes { get; set; } = new();

        public Timer timer = new(5000);

        public ObservableCollection<ObservedProcess> ObservedProcesses { get; set; } = new();

        public ObservableCollection<ProcessObserver> ProcessObservers { get; set; } = new();

        public MainStore()
        {
            LoadProcessList();
            LoadObservedProcesses();
        }

        public void LoadProcessList()
        {
            Processes.Clear();
            var processes = Process.GetProcesses();
            foreach (var process in processes)
            {
                var item = new ProcessItem { Name = process.ProcessName, Id = process.Id, };

                Processes.Add(item);
            }
        }

        public void LoadObservedProcesses()
        {
            ObservedProcesses.Clear();
            foreach (var item in ProcessObservers)
            {
                item.StopObserving();
            }
            ProcessObservers.Clear();

            var observedProcessesFromDb = new GetAllObservedProcessesQuery().Execute().ToList();
            foreach (var item in observedProcessesFromDb)
            {

                ObservedProcessVM itemVM = new(item.Name)
                {
                    ClosedAt = item.ClosedAt,
                    CreatedAt = item.CreatedAt,
                    TotalSpent = item.TotalSpent,
                    Name = item.Name,
                    OpenedAt = item.OpenedAt
                };
                ObservedProcesses.Add(itemVM); 
                var observer = new ProcessObserver(
                    itemVM,
                    onProcessClosedAction: OnProcessClosed,
                    onProcessStarted: OnProcessOpened
                );
                _ = observer.StartObservingAsync();
                ProcessObservers.Add(observer);
            }
        }

        public void SaveChanges()
        {
            new UpdateObservedProcessesCommand(ObservedProcesses).Execute();
        }

        public void StopObservingAll()
        {
            foreach (var item in ProcessObservers)
            {
                item.StopObserving();
            }
        }

        public void CloseUnclosed()
        {
            foreach (var item in ObservedProcesses)
            {
                if (item.ClosedAt > item.OpenedAt)
                {
                    continue;
                }
                item.ClosedAt = DateTime.Now;
                item.TotalSpent += item.ClosedAt - item.OpenedAt;
            }
        }

        public void AddObservedProcess(string processName)
        {
            if (ObservedProcesses.Any(x => x.Name == processName))
            {
                return;
            }
            var tempProcess = new ObservedProcessVM(processName);
            tempProcess.OpenedAt = SetStartUpTimeOrReturnDefaultValue(processName);
            ObservedProcesses.Add(tempProcess);
            ProcessObservers.Add(
                new ProcessObserver(tempProcess, OnProcessClosed, OnProcessOpened)
            );
            new AddObservedProcessCommand(tempProcess).Execute();
            _ = ProcessObservers.Last().StartObservingAsync();
        }

        public void RemoveObservedProcess(string processName)
        {
            if (!ObservedProcesses.Any(x => x.Name == processName))
            {
                return;
            }
            var observer = ProcessObservers.FirstOrDefault(
                x => x.TargetProcess.Name == processName
            );
            if (observer == null)
            {
                return;
            }
            observer.StopObserving();
            ProcessObservers.Remove(observer);

            var targetProcess = ObservedProcesses.FirstOrDefault(x => x.Name == processName);
            if (targetProcess == null)
            {
                throw new Exception("No any process with what name");
            }
            ObservedProcesses.Remove(targetProcess);
            new RemoveObservedProcessCommand(targetProcess).Execute();
        }

        public DateTime SetStartUpTimeOrReturnDefaultValue(string processName)
        {
            var processes = Process.GetProcessesByName(processName);
            if (processes.Length > 0)
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

        public void OnProcessClosed(
            ProcessObserver observer,
            ObservedProcess observedProcess,
            DateTime closedTime
        )
        {
            observedProcess.ClosedAt = closedTime;
            observedProcess.TotalSpent += observedProcess.ClosedAt - observedProcess.OpenedAt;
            new UpdateObservedProcessCommand(observedProcess).Execute();
            /*MessageBox.Show($"observer notices that {observedProcess.Name} was closed at {closedTime}");*/
        }

        public void OnProcessOpened(
            ProcessObserver observer,
            ObservedProcess observedProcess,
            DateTime openedTime
        )
        {
            observedProcess.OpenedAt = openedTime;
            new UpdateObservedProcessCommand(observedProcess).Execute();
            /*MessageBox.Show($"observer notices that {observedProcess.Name} was opened at {openedTime}");*/
        }
    }
}
