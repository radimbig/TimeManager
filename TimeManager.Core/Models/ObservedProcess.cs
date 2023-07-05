namespace TimeManager.Core.Models
{
    public class ObservedProcess
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime OpenedAt { get; set; }

        public DateTime ClosedAt { get; set; }

        public DateTime TotalSpend { get; set; }

        public string Name { get; set; }

        public ObservedProcess(string name)
        {
            Name = name;
            CreatedAt = DateTime.Now;
        }
    }
}
