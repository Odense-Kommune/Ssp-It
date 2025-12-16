using Dk.Odense.SSP.Gdf.Model.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dk.Odense.SSP.Gdf
{
    public class GdfDbContext<TValue> : DbContext where TValue : class, IGdfEntity, new()
    {
        public GdfDbContext(DbContextOptions<GdfDbContext<TValue>> options) : base(options)
        {

        }

        public DbSet<TValue> Table { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
