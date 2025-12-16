using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Dk.Odense.SSP.Logger
{
    public class LogDbContext : DbContext
    {
        public LogDbContext(DbContextOptions<LogDbContext> options) : base(options)
        {

        }

        public DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Log>().Property(x => x.Id).Metadata.ValueGenerated = ValueGenerated.OnAdd;

            base.OnModelCreating(modelBuilder);
        }
    }
}
