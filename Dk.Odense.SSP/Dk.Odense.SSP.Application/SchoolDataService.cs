using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.Domain.Interfaces;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Application
{
    public class SchoolDataService : BaseService<InternalSchoolData, ISchoolDataRepository>, ISchoolDataService
    {
        public SchoolDataService(ISchoolDataRepository repository) : base(repository)
        {
            this.Repository = repository;
        }
    }
}
