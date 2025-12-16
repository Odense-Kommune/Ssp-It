using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dk.Odense.SSP.Domain.Model;
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
    public class AgendaOverviewController : ControllerBase
    {
        private readonly AgendaOverviewUseCase agendaOverviewUseCase;
        private readonly HangfireManager hangfireManager;
        private readonly ILoggerService loggerService;
        private readonly string user;

        public AgendaOverviewController(AgendaOverviewUseCase agendaOverviewUseCase, ILoggerService loggerService, HangfireManager hangfireManager, IHttpContextAccessor contextAccessor)
        {
            this.agendaOverviewUseCase = agendaOverviewUseCase;
            this.loggerService = loggerService;
            this.hangfireManager = hangfireManager;
            var userContext = contextAccessor.HttpContext?.User ?? null;
            user = userContext?.Claims.FirstOrDefault(c => c.Type == "preferred_username")?.Value ?? "Hangfire";
            user = user.Replace("@odense.dk", "");
        }

        [HttpGet]
        public async Task<IEnumerable<Agenda>> GetHeldAgendas()
        {
            return await agendaOverviewUseCase.GetHeldAgendas();
        }

        [HttpGet]
        public async Task<IEnumerable<Agenda>> GetCurrentAgendas()
        {
            return await agendaOverviewUseCase.GetCurrentAgendas();
        }

        [HttpPut]
        public async Task<bool> UpdateAgenda(Agenda agenda)
        {
            return await agendaOverviewUseCase.UpdateAgenda(agenda);
        }

        [HttpGet]
        public async Task<IEnumerable<PendingWorry>> GetPendingWorries()
        {
            var ret = await agendaOverviewUseCase.GetPendingWorries();

            hangfireManager.EnqueueJob(() => loggerService.LogHangfire(user, ControllerContext.RouteData.Values["controller"] + "/" + ControllerContext.RouteData.Values["action"], ret.Select(x => string.Concat("Cpr:", x.Person.SocialSecNum)).ToList()));

            return ret;
        }

        [HttpPost]
        public async Task<Agenda> CreateNewAgenda(Agenda agenda)
        {
            return await agendaOverviewUseCase.CreateNewAgenda(agenda);
        }

        [HttpPut]
        public Agenda UpdateMeetDate(Agenda agenda)
        {
            return agendaOverviewUseCase.UpdateMeetDate(agenda);
        }

        [HttpPost]
        public async Task<bool> CreateAgendaItems(IEnumerable<PendingWorry> items)
        {
            return await agendaOverviewUseCase.CreateAgendaItems(items);
        }
    }
}