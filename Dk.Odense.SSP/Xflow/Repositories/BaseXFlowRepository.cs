using Dk.Odense.SSP.Core;
using Dk.Odense.SSP.Infrastructure.Interfaces;
using Dk.Odense.SSP.Xflow.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dk.Odense.SSP.Xflow.Repositories
{
    public class BaseXFlowRepository<TValue> : IBaseXFlowRepository<TValue> where TValue : class, IEntity, new()
    {
        protected readonly IDatabaseContext DatabaseContext;
        protected readonly DbSet<TValue> DbSet;

        public BaseXFlowRepository(IDatabaseContext databaseContext)
        {
            DatabaseContext = databaseContext;
            DbSet = databaseContext.Set<TValue>();
        }
        public virtual async Task<List<TValue>?> Create(List<TValue> values)
        {
            if (values.Count == 1)
            {
                await DbSet.AddAsync(values[0]);
            }
            else if (values.Count == 0)
            {
                //do nothing
            }
            else
            {
                await DbSet.AddRangeAsync(values);
            }
            return values;
        }

        public virtual async Task<List<TValue>> List()
        {
            return await DbSet.AsNoTracking().ToListAsync();
        }

        public virtual async Task<bool> FindDuplicate(Guid id)
        {
            var result = await DbSet.FindAsync(id);
            return result != null;
        }


    }
}
