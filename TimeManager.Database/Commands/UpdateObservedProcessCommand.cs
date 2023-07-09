using TimeManager.Core.Models;
using TimeManager.Database.Interfaces;

namespace TimeManager.Database.Commands
{
    public class UpdateObservedProcessCommand : ITimeManagerCommand
    {
        ObservedProcess _target;

        UpdateObservedProcessCommand(ObservedProcess process)
        {
            _target = process;
        }

        public void Execute()
        {
            /*            using (var ctx = new TimeManagerDbContext())
                        {
                            if (!ctx.ObservedProcesses.Any(x => x.Name == _target.Name))
                            {
                                throw new Exception($"No any observed process with name {_target.Name} in database to update");
                            }
                            var similar = ctx.ObservedProcesses.FirstOrDefault(x => x.Name == _target.Name);
                            if (similar is null)
                            {
                                throw new Exception("Db connection error");
                            }
                            _target.Id = similar.Id;
            
                        }*/
            throw new NotImplementedException();
        }
    }
}
