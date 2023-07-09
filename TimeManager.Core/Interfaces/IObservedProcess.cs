using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManager.Core.Interfaces
{
    public interface IObservedProcess
    {
        DateTime ClosedAt { get; set; }
        DateTime CreatedAt { get; set; }
        int Id { get; set; }
        string Name { get; set; }
        DateTime OpenedAt { get; set; }
        TimeSpan TotalSpent { get; set; }
    }
}
