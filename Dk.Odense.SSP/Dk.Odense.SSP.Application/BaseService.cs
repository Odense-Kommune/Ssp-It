using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.Core;
using Dk.Odense.SSP.Domain.Interfaces;

namespace Dk.Odense.SSP.Application
{
    public abstract class BaseService<TValue, TRepository> : IBaseService<TValue> where TRepository : IBaseRepository<TValue>
                                                                                where TValue : class, IEntity, new()
    {
        protected TRepository Repository;

        public BaseService(TRepository repository)
        {
            this.Repository = repository;
        }

        public async Task<TValue> Get(Guid id)
        {
            return await Repository.Get(id);
        }

        public async Task<IEnumerable<TValue>> List()
        {
            return await Repository.List();
        }

        public async Task<TValue> Create(TValue value)
        {
            await Repository.Create(value);
            return value;
        }

        public TValue Update(TValue value)
        {
            Repository.Update(value);
            return value;
        }

        public async Task<bool> Delete(Guid id)
        {
            await Repository.Delete(id);
            return true;
        }

        //Use this to check if there's an entry in the database with the same Id
        public virtual async Task<bool> FindDuplicate(Guid id)
        {
            return await Repository.FindDuplicate(id);
        }
    }
}
