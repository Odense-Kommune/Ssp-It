using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dk.Odense.SSP.Core;
using Dk.Odense.SSP.Domain.Model;
using Dk.Odense.SSP.ExternalServices.Interfaces;
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
    public class PersonController : ControllerBase
    {
        private readonly PersonUseCase personUseCase;
        private readonly HangfireManager hangfireManager;
        private readonly ILoggerService loggerService;
        private readonly ICprService cprService;
        private readonly string user;

        public PersonController(PersonUseCase personUseCase, ILoggerService loggerService, ICprService cprService, HangfireManager hangfireManager, IHttpContextAccessor contextAccessor)
        {
            this.personUseCase = personUseCase;
            this.loggerService = loggerService;
            this.cprService = cprService;
            this.hangfireManager = hangfireManager;

            var userContext = contextAccessor.HttpContext?.User ?? null;
            user = userContext?.Claims.FirstOrDefault(c => c.Type == "preferred_username")?.Value ?? "Hangfire";
            user = user.Replace("@odense.dk", "");
        }

        [HttpGet("{id}")]
        public async Task<PersonDto> Get(Guid id)
        {
            var ret = await personUseCase.Get(id);

            hangfireManager.EnqueueJob(() => loggerService.LogHangfire(user, ControllerContext.RouteData.Values["controller"] + "/" + ControllerContext.RouteData.Values["action"], string.Concat("Cpr:", ret.SocialSecNum)));



            return ret;
        }

        [HttpGet]
        public async Task<IEnumerable<PersonDto>> List()
        {
            var ret = await personUseCase.List();

            hangfireManager.EnqueueJob(() => loggerService.LogHangfire(user, ControllerContext.RouteData.Values["controller"] + "/" + ControllerContext.RouteData.Values["action"], ret.Select(x => string.Concat("Cpr:", x.SocialSecNum)).ToList()));

            return ret;
        }

        [HttpPut]
        public async Task<bool> SetSspArea(Guid id, Guid sspAreaId)
        {
            return await personUseCase.SetSspArea(id, sspAreaId);
        }

        [HttpPut]
        public async Task<bool> SetSocialWorker(Guid id, string socialWorker)
        {
            return await personUseCase.SetSocialWorker(id, socialWorker);
        }

        [HttpPut("{id}")]
        public async Task<bool> SetSspStopDate(Guid id, [FromBody] DateTime date)
        {
            return await personUseCase.SetSspStopDate(id, date);
        }

        [HttpPut("{id}")]
        public async Task<bool> DeleteSspStopDate(Guid id)
        {
            return await personUseCase.DeleteSspStopDate(id);
        }

        [HttpGet]
        public async Task<NavneOpslagData> GetSocialSecData(string socialSecNum)
        {
            var ret = await cprService.GetPerson(socialSecNum);

            hangfireManager.EnqueueJob(() => loggerService.LogHangfire(user, ControllerContext.RouteData.Values["controller"] + "/" + ControllerContext.RouteData.Values["action"], string.Concat("Cpr:", ret.SocialSecNum)));

            return ret;
        }

        [HttpGet]
        public async Task<IEnumerable<Person>> SearchGroupAndName(string term)
        {
            var ret = await personUseCase.SearchGroupAndName(term);

            hangfireManager.EnqueueJob(() => loggerService.LogHangfire(user, ControllerContext.RouteData.Values["controller"] + "/" + ControllerContext.RouteData.Values["action"], ret.Select(x => string.Concat("Cpr:", x.SocialSecNum)).ToList()));

            return ret;
        }

        [HttpGet]
        public async Task<IEnumerable<Person>> SearchCpr(string term)
        {
            var ret = await personUseCase.SearchCpr(term);

            hangfireManager.EnqueueJob(() => loggerService.LogHangfire(user, ControllerContext.RouteData.Values["controller"] + "/" + ControllerContext.RouteData.Values["action"], ret.Select(x => string.Concat("Cpr:", x.SocialSecNum)).ToList()));

            return ret;
        }

        [HttpGet]
        public async Task<Categorization> GetLatestCategorization(Guid id)
        {
            return await personUseCase.GetLatestCategorization(id);
        }
    }
}