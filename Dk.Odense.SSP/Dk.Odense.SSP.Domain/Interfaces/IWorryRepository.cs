using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Domain.Interfaces
{
    public interface IWorryRepository : IBaseRepository<Worry>
    {
        Task<int> GetCountByPersonId(Guid personId);
        Task<Worry> GetWithAgendaItems(Guid id);
        Task<Worry> GetNext(Guid personId, int increment);
        Task<Worry> GetPrevious(Guid personId, int increment);
        Task<IEnumerable<Worry>> GetToBeVerified();
        Task<IEnumerable<Worry>> GetToBeVerifiedWithIncludes();
        Task<IEnumerable<Worry>> GetFromPersonId(Guid personId);
        Task<IEnumerable<Worry>> GetNotPendingWithIncludes();
        Task<Worry> GetWithIncludes(Guid id);
        Task<IEnumerable<Worry>> GetGroundless();
    }
}
