using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Application.Interfaces
{
    public interface IWorryService : IBaseService<Worry>
    {
        Task<int> GetCountByPersonId(Guid personId);
        Task SetGroundless(Guid id);
        Task<IEnumerable<Worry>>  GetGroundless();
        Task<bool> Unverify(Guid id);
        Task<bool> SetPoliceWorryRole(Guid id, Guid policeWorryRoleId);
        Task<bool> SetPoliceWorryCategory(Guid id, Guid policeWorryCategoryId);
        Task<Guid> AgendaItemCleanup(Guid id);
        Task<Worry> GetNext(Guid personId, int increment);
        Task<Worry> GetPrevious(Guid personId, int increment);
        Task<IEnumerable<Worry>> GetToBeVerified();
        Task<IEnumerable<Worry>> GetToBeVerifiedWithIncludes();
        Task<IEnumerable<Worry>> GetFromPersonId(Guid personId);
        Task<IEnumerable<Worry>> GetNotPendingWithIncludes();
        Task<Worry> GetWithIncludes(Guid id);
    }
}
