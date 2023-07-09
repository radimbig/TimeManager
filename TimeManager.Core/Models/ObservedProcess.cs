using System.Diagnostics;
using TimeManager.Core.Interfaces;
namespace TimeManager.Core.Models
{
    public class ObservedProcess : IObservedProcess
    {
        public int Id { get; set; }
        public virtual DateTime CreatedAt { get; set; }

        public virtual DateTime OpenedAt { get; set; }

        public virtual DateTime ClosedAt { get; set; }

        public virtual TimeSpan TotalSpent { get; set; }

        public string Name { get; set; }

        public ObservedProcess(string name)
        {
            Name = name;
            CreatedAt = DateTime.Now;
        }
    }

}
