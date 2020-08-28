using Microsoft.EntityFrameworkCore;

namespace athene
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Entry> Entries { get; set; }

        public DatabaseContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite("DataSource=entries.db");
    }
}