using Microsoft.EntityFrameworkCore;
using TaskManager.Entities;

namespace TaskManager.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<TaskItem> TaskItems { get; set; }

        private readonly string _connectionString;
        public DatabaseContext(DbContextOptions<DatabaseContext> options, IConfiguration configuration) : base(options)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
