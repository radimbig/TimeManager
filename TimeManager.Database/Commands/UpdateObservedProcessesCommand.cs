using TimeManager.Core.Models;
using TimeManager.Database.Interfaces;

namespace TimeManager.Database.Commands
{
    public class UpdateObservedProcessesCommand : ITimeManagerCommand
    {
        IEnumerable<ObservedProcess> _observedProcesses;
        public UpdateObservedProcessesCommand(IEnumerable<ObservedProcess> observedProcesses) { _observedProcesses = observedProcesses; }
        public void Execute()
        {
            foreach(var pr in _observedProcesses)
            {
                new UpdateObservedProcessCommand(pr).Execute();
            }
        }
    }
}
