using Dk.Odense.SSP.Sbsys.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dk.Odense.SSP.Web.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SBSYSController : ControllerBase
    {
        private readonly ISbsysCaseService sbsysCaseService;

        public SBSYSController(ISbsysCaseService sbsysCaseService)
        {
            this.sbsysCaseService = sbsysCaseService;
        }
    }
}