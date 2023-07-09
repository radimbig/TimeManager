using TimeManager.Core.Models;
using TimeManager.Database.Interfaces;

namespace TimeManager.Database.Queries
{
    internal class GetAllObservedProcessesQuery : ITimeManagerQuery<IEnumerable<ObservedProcess>>
    {
        public IEnumerable<ObservedProcess> Execute()
        {
            using(var ctx = new TimeManagerDbContext())
            {
                return ctx.ObservedProcesses.ToList();
            }
        }
    }
}
