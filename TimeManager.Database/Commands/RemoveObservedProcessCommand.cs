using TimeManager.Core.Models;
using TimeManager.Database.Interfaces;

namespace TimeManager.Database.Commands
{
    public class RemoveObservedProcessCommand : ITimeManagerCommand
    {

        private ObservedProcess _target;
        public RemoveObservedProcessCommand(ObservedProcess pr)
        {
            _target = pr;
        }

        public void Execute()
        {
            using(var ctx = new TimeManagerDbContext())
            {
                if(!ctx.ObservedProcesses.Any(x=>x.Name == _target.Name))
                {
                    return;
                }
                var dbIntance = ctx.ObservedProcesses.FirstOrDefault(x=>x.Name == _target.Name);
                if (dbIntance == null)
                {
                    throw new Exception("Db error when deleting");
                }
                ctx.ObservedProcesses.Remove(dbIntance);
                ctx.SaveChanges();
            }
        }
    }
}
