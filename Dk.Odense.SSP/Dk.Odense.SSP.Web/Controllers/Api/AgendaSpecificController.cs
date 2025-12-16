using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dk.Odense.SSP.Logger;
using Dk.Odense.SSP.UserCase;
using Dk.Odense.SSP.UserCase.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dk.Odense.SSP.Web.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Policy = "SSP")]
    public class AgendaSpecificController : ControllerBase
    {
        private readonly AgendaSpecificUseCase agendaSpecificUseCase;
        private readonly HangfireManager hangfireManager;
        private readonly ILoggerService loggerService;
        private readonly string user;

        public AgendaSpecificController(AgendaSpecificUseCase agendaSpecificUseCase, ILoggerService loggerService, HangfireManager hangfireManager, IHttpContextAccessor contextAccessor)
        {
            this.agendaSpecificUseCase = agendaSpecificUseCase;
            this.loggerService = loggerService;
            this.hangfireManager = hangfireManager;

            var userContext = contextAccessor.HttpContext?.User ?? null;
            user = userContext?.Claims.FirstOrDefault(c => c.Type == "preferred_username")?.Value ?? "Hangfire";
            user = user.Replace("@odense.dk", "");
        }

        [HttpGet]
        public async Task<IEnumerable<AgendaSpecificMenuItem>> GetMenuItems(Guid agendaId)
        {
            var ret = await agendaSpecificUseCase.GetMenuItems(agendaId);

            hangfireManager.EnqueueJob(() => loggerService.LogHangfire(user, ControllerContext.RouteData.Values["controller"] + "/" + ControllerContext.RouteData.Values["action"], ret.Select(x => string.Concat("Cpr:", x.Person.SocialSecNum)).ToList()));

            return ret;
        }
    }
}