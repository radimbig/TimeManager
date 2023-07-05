namespace TimeManager.Core
{
    public class ObservedTask
    {
        public DateTime CreatedAt { get; set; }

        public DateTime OpenedAt { get; set; }

        public DateTime ClosedAt { get; set; }

        public DateTime TotalSpend { get; set; }

        public string Name { get;set; }
        
        public ObservedTask(string name)
        {
            Name = name;
            CreatedAt = DateTime.Now;
        }
    }
}
