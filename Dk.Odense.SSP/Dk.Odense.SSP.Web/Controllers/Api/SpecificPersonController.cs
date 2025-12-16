using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dk.Odense.SSP.Logger;
using Dk.Odense.SSP.UserCase;
using Dk.Odense.SSP.UserCase.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dk.Odense.SSP.Web.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Policy = "SSP")]
    public class SpecificPersonController : ControllerBase
    {
        private readonly SpecificPersonUseCase specificPersonUseCase;
        private readonly ILoggerService loggerService;

        public SpecificPersonController(SpecificPersonUseCase specificPersonUseCase, ILoggerService loggerService)
        {
            this.specificPersonUseCase = specificPersonUseCase;
            this.loggerService = loggerService;
        }

        [HttpGet]
        public async Task<IEnumerable<PersonMenuItem>> GetMenuItems(Guid personId)
        {
            var ret = await specificPersonUseCase.GetMenuItems(personId);

            return ret;
        }
    }
}