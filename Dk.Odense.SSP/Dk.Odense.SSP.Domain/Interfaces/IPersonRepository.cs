using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dk.Odense.SSP.Core;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Domain.Interfaces
{
    public interface IPersonRepository : IBaseRepository<Person>
    {
        Task<Person> GetBySocialSecNum(string socialSecNum);
        void SetSspStopDate(Person person);
        Task<InternalSchoolData> GetSchoolData(Guid personId);
        Task<Person> GetPersonWithIncludes(Guid id);
        Task<IEnumerable<Person>> GetPersonWithPendingWorries();
        Task<Worry> GetLatestCategorization(Guid id);
        Task<IEnumerable<Person>> GetPersonsWithIncludes();
        Task<IEnumerable<Person>> SearchCpr(string term);
        Task<IEnumerable<Person>> SearchGroupAndName(string term);
        Task<IEnumerable<DeletePerson>> GetPersonsForDeleting();
    }
}
