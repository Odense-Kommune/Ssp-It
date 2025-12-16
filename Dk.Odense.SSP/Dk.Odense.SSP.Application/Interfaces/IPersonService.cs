using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dk.Odense.SSP.Core;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Application.Interfaces
{
    public interface IPersonService : IBaseService<Person>
    {
        Task<Person> GetBySocialSecNum(string socialSecNum);
        void SetSspStopDate(Guid id, DateTime date);
        Task SetSspArea(Guid id, Guid sspAreaId);
        Task<Categorization> GetLatestCategorization(Guid id);
        Task<InternalSchoolData> GetSchoolData(Guid personId);
        Task UpdateSchoolData(Person person);
        Task<IEnumerable<Person>> GetPersonWithPendingWorries();
        Task<Person> GetPersonWithIncludes(Guid id);
        Task<IEnumerable<Person>> GetPersonsWithIncludes();
        Task<IEnumerable<Person>> SearchCpr(string term);
        Task<IEnumerable<Person>> SearchGroupAndName(string term);
        void DeleteSspStopDate(Guid id);
        Task<IEnumerable<DeletePerson>> GetPersonsForDeleting(DateTime date);
        Task SetSocialWorker(Guid id, string socialWorker);
    }
}
