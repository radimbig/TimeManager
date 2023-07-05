using Microsoft.EntityFrameworkCore;
using TimeManager.Core.Models;

namespace TimeManager.Database
{
    public class TimeManagerDbContext : DbContext
    {
        private string ConnectionString = "Data Source=TimeManagerDatabase.db";
        DbSet<ObservedProcess> ObservedProcesses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
