using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.Domain.Model;
using System;
using System.Threading.Tasks;

namespace Dk.Odense.SSP.UserCase
{
    public class SchoolDataUseCase
    {
        ISchoolDataService schoolDataService;
        IPersonService personService;
        public SchoolDataUseCase(ISchoolDataService schoolDataService, IPersonService personService)
        {
            this.schoolDataService = schoolDataService;
            this.personService = personService;
        }

        public async Task<InternalSchoolData> GetSchoolDataByPersonId(Guid personId)
        {
            return await personService.GetSchoolData(personId);
        }
    }
}
