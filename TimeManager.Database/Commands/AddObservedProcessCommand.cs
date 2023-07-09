using TimeManager.Core;
using TimeManager.Core.Models;
using TimeManager.Database.Interfaces;

namespace TimeManager.Database.Commands
{
    public class AddObservedProcessCommand : ITimeManagerCommand
    {
        private readonly ObservedProcess targetProcess;
        public AddObservedProcessCommand(ObservedProcess pr) { targetProcess = pr; }

        public void Execute()
        {
            using(var ctx = new TimeManagerDbContext())
            {
                if(ctx.ObservedProcesses.Any(x=>x.Name == targetProcess.Name))
                {
                    return;
                }
                ctx.Add(targetProcess);
                ctx.SaveChanges();
            }
        }
    }
}
