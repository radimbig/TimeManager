using System.Diagnostics;

namespace TimeManager.Core.Models
{
    public class ProcessObserver : IDisposable
    {
        private readonly CancellationTokenSource cancellationTokenSource = new();
        public ObservedProcess TargetProcess { get; private set; }
        public event Action<ProcessObserver, ObservedProcess, DateTime>? OnProcessClosed;
        public event Action<ProcessObserver, ObservedProcess, DateTime>? OnProcessStarted;
        public ProcessStatus CurrentStatus { get; set; } = ProcessStatus.NotStarted;
        public int RefreshTime;

        public ProcessObserver(
            ObservedProcess targetProcess,
            Action<ProcessObserver, ObservedProcess, DateTime> onProcessClosedAction,
            Action<ProcessObserver, ObservedProcess, DateTime> onProcessStarted,
            int refreshTime = 5000
        )
        {
            OnProcessClosed += onProcessClosedAction;
            OnProcessStarted += onProcessStarted;
            TargetProcess = targetProcess;
            RefreshTime = refreshTime;
        }

        public async Task StartObservingAsync()
        {
            await Task.Run(
                () =>
                {
                    StartObserving();
                },
                cancellationTokenSource.Token
            );
        }

        public void StopObserving()
        {
            cancellationTokenSource.Cancel();
        }

        private void StartObserving()
        {
            Trace.WriteLine($"Observing was started {DateTime.Now}");
            while(!cancellationTokenSource.IsCancellationRequested)
            {
                Trace.WriteLine("Status checking is starting...");
                if (GetStatus(TargetProcess.Name) == ProcessStatus.Running)
                {
                    ProcessIsRunning();
                }
                if(GetStatus(TargetProcess.Name) == ProcessStatus.Stoped)
                {
                    ProcessIsStoped();
                }
                Trace.WriteLine($"Process status is {CurrentStatus}");

                
                for (int i = 0; i < RefreshTime / 100; i++)
                {
                    if (cancellationTokenSource.IsCancellationRequested)
                    {
                        break;
                    }
                    Thread.Sleep(100);
                }

            }
            Trace.WriteLine($"Observation was interupted {DateTime.Now}");
        }

        private void ProcessIsStoped()
        {
            if(CurrentStatus == ProcessStatus.NotStarted)
            {
                return;
            }
            if(CurrentStatus != ProcessStatus.Stoped)
            {
                OnProcessClosed?.Invoke(this, TargetProcess, DateTime.Now);
                CurrentStatus = ProcessStatus.Stoped;
            }
        }
        private void ProcessIsRunning()
        {
            if (CurrentStatus != ProcessStatus.Running)
            {
                OnProcessStarted?.Invoke(this, TargetProcess, DateTime.Now);
                CurrentStatus = ProcessStatus.Running;
            }
        }

        private ProcessStatus GetStatus(string name)
        {
            if (IsProcessRunning(name))
            {
                return ProcessStatus.Running;
            }
            return ProcessStatus.Stoped;
        }

        public void WaitUntilClosed(string processName, CancellationToken token)
        {
            while (!token.IsCancellationRequested) 
            {
                if(!IsProcessRunning(processName))
                {
                    return;
                }
                Thread.Sleep(RefreshTime);
            }
        }

        public void WaitUntilStarted(string processName, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if(IsProcessRunning(processName))
                {
                    return;
                }
                Thread.Sleep(RefreshTime);
            }
        }

        public bool IsProcessRunning(string processName)
        {
            Trace.WriteLine("IsProcessRunning was called","Info");
            Process[] processes = Process.GetProcessesByName(processName);
            Trace.WriteLine($"Processes count {processes.Length}");
            return processes.Length > 0;
        }

        public void Dispose()
        {
            OnProcessClosed = null;
            OnProcessStarted = null;
        }

        public enum ProcessStatus
        {
            Running,
            Stoped,
            NotStarted
        }
    }
}
