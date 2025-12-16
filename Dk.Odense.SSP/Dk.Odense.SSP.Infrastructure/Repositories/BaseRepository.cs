using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dk.Odense.SSP.Core;
using Dk.Odense.SSP.Domain.Interfaces;
using Dk.Odense.SSP.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dk.Odense.SSP.Infrastructure.Repositories
{
    public class BaseRepository<TValue> : IBaseQueryRepository<TValue> where TValue : class, IEntity, new()
    {
        protected readonly IDatabaseContext DatabaseContext;
        protected readonly DbSet<TValue> DbSet;

        public BaseRepository(IDatabaseContext databaseContext)
        {
            DatabaseContext = databaseContext;
            DbSet = databaseContext.Set<TValue>();
        }

        public async Task<TValue> Get(Guid id)
        {
            return await DbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public virtual async Task<IEnumerable<TValue>> List()
        {
            return await ListQuery().ToListAsync();
        }

        public virtual IQueryable<TValue> ListQuery()
        {
            return DbSet.AsNoTracking();
        }

        public virtual async Task<TValue> Create(TValue value)
        {
            await DbSet.AddAsync(value);
            return value;
        }

        public virtual TValue Update(TValue value)
        {
            var entity = DbSet.Attach(value);

            DbSet.Update(value);
            entity.State = EntityState.Modified;

            return value;
        }

        public virtual async Task<bool> Delete(Guid id)
        {
            var entity = await Get(id);

            if (entity == null) return false;

            var entityState = DbSet.Attach(entity);
            entityState.State = EntityState.Deleted;

            DbSet.Remove(entity);
            return true;
        }

        public virtual async Task<bool> FindDuplicate(Guid id)
        {
            var result = await DbSet.FindAsync(id);
            return result != null;
        }
    }
}
