using System;
using System.Threading.Tasks;
using Dk.Odense.SSP.Domain.Model;
using Dk.Odense.SSP.UserCase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dk.Odense.SSP.Web.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Policy = "SSP")]
    public class SchoolDataController : ControllerBase
    {
        private readonly SchoolDataUseCase schoolDataUseCase;

        public SchoolDataController(SchoolDataUseCase schoolDataUseCase)
        {
            this.schoolDataUseCase = schoolDataUseCase;
        }

        [HttpGet]
        public async Task<InternalSchoolData> GetSchoolData(Guid personId)
        {
            try
            {
                var schoolData = await schoolDataUseCase.GetSchoolDataByPersonId(personId) ?? new InternalSchoolData() { Name = "Skole Ikke Fundet" };

                return schoolData;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}