using System.Threading.Tasks;
using Dk.Odense.SSP.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Dk.Odense.SSP.Web.Controllers.Api
{
    [ApiController]
    [Route("account/[action]")]
    public class AccountController : ControllerBase
    {
        private readonly HangfireManager hangfireManager;
        private readonly ILoggerService loggerService;
        private readonly IWebHostEnvironment env;

        public AccountController(HangfireManager hangfireManager, IWebHostEnvironment env, ILoggerService loggerService)
        {
            this.hangfireManager = hangfireManager;
            this.env = env;
            this.loggerService = loggerService;
        }

        [HttpGet]
        [Authorize("SSP")]
        public async Task<bool> IsAuthorized()
        {
            var r = HttpContext.User.Identity.IsAuthenticated;

            return r;
        }
    }
}
