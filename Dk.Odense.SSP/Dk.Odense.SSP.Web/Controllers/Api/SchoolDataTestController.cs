using Dk.Odense.SSP.UserCase;
using Microsoft.AspNetCore.Mvc;

namespace Dk.Odense.SSP.Web.Controllers.Api
{
    //[Route("api/[controller]/[action]")]
    //[ApiController]
    public class SchoolDataTestController : ControllerBase
    {
        private readonly HangfireManager hangfireManager;
        private readonly HangFireUseCase hangFireUseCase;

        public SchoolDataTestController(HangfireManager hangfireManager, HangFireUseCase hangFireUseCase)
        {
            this.hangfireManager = hangfireManager;
            this.hangFireUseCase = hangFireUseCase;
        }
    }
}