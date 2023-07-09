using TimeManager.Core.Models;
using TimeManager.Database.Interfaces;

namespace TimeManager.Database.Commands
{
    public class UpdateObservedProcessCommand : ITimeManagerCommand
    {
        ObservedProcess _target;

        public UpdateObservedProcessCommand(ObservedProcess process)
        {
            _target = process;
        }

        public void Execute()
        {
            using(var ctx = new TimeManagerDbContext())
            {
                if(!ctx.ObservedProcesses.Any(x=>x.Name == _target.Name))
                {
                    throw new Exception($"No any observed process with name{_target.Name}");
                }
                var dbInstance = ctx.ObservedProcesses.FirstOrDefault(x=>x.Name == _target.Name);
                if(dbInstance == null) 
                {
                    throw new Exception("Db error");
                }
                _target.Id = dbInstance.Id;
                ctx.ObservedProcesses.Entry(dbInstance).CurrentValues.SetValues(_target);
                ctx.SaveChanges();
            }
        }
    }
}
