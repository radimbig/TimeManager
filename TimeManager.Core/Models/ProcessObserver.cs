using System.Diagnostics;

namespace TimeManager.Core.Models
{
    public class ProcessObserver
    {
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        public ObservedProcess TargetProcess { get; private set; }
        public event Action<ProcessObserver, ObservedProcess, DateTime> OnProcessClosed;

        public int RefreshTime;

        public ProcessObserver(ObservedProcess targetProcess, Action<ProcessObserver, ObservedProcess, DateTime> action, int refreshTime = 5000)
        {
            OnProcessClosed = action;
            TargetProcess = targetProcess;
            RefreshTime = refreshTime;
        }

        public async Task StartObservingAsync()
        {
            await Task.Run(() => { StartObserving(); }, cancellationTokenSource.Token);
        }

        public void StopObserving()
        {
            cancellationTokenSource.Cancel();
        }

        private void StartObserving()
        {
            while (!cancellationTokenSource.IsCancellationRequested)
            {
                if (!IsProcessRunning(TargetProcess.Name))
                {
                    TargetProcess.ClosedAt = DateTime.Now;
                    OnProcessClosed(this, TargetProcess, DateTime.Now);
                }
                Thread.Sleep(RefreshTime);
            }
        }

        public bool IsProcessRunning(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            return processes.Length > 0;
        }
    }
}
