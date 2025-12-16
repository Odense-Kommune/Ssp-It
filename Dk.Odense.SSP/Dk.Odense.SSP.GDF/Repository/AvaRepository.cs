using System;
using System.Linq;
using Dk.Odense.SSP.Gdf.Model.Interfaces;
using Dk.Odense.SSP.Gdf.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Dk.Odense.SSP.Gdf.Repository
{
    public class AvaRepository<TValue> : IGdfRepository<TValue> where TValue : class, IAva, new()
    {
        private readonly GdfDbContext<TValue> databaseContext;
        private readonly DbSet<TValue> dbSet;

        public AvaRepository(GdfDbContext<TValue> databaseContext)
        {
            this.databaseContext = databaseContext;
            dbSet = databaseContext.Set<TValue>();
        }

        public TValue Get(Guid id)
        {
            return dbSet.FirstOrDefault(x => x.ID == id);
        }

        public IQueryable<TValue> List()
        {
            return dbSet.AsNoTracking();
        }

        public bool Delete(Guid id)
        {
            try
            {
                var entity = Get(id);

                dbSet.Remove(entity);

                databaseContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;
        }
    }
}
