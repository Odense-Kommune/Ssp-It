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
    public class VerifyWorryController : ControllerBase
    {
        private readonly VerifyWorryUseCase verifyWorryUseCase;
        private readonly HangfireManager hangfireManager;
        private readonly ILoggerService loggerService;
        private readonly string user;

        public VerifyWorryController(
            VerifyWorryUseCase verifyWorryUseCase, 
            HangfireManager hangfireManager, 
            ILoggerService loggerService, 
            IHttpContextAccessor contextAccessor)
        {
            this.verifyWorryUseCase = verifyWorryUseCase;
            this.hangfireManager = hangfireManager;
            this.loggerService = loggerService;

            var userContext = contextAccessor.HttpContext?.User ?? null;
            user = userContext?.Claims.FirstOrDefault(c => c.Type == "preferred_username")?.Value ?? "Hangfire";
            user = user.Replace("@odense.dk", "");
        }

        [HttpGet("{id}")]
        public async Task<VerifyWorry> Get(Guid id)
        {
            var ret = await verifyWorryUseCase.GetVerifyWorry(id);

            var reqParm = ret.SocialSecNum != string.Empty ? string.Concat("Cpr:", ret.SocialSecNum) : string.Concat("Reported Cpr:", ret.ReportedPerson.ReportedCpr);

            hangfireManager.EnqueueJob(() => loggerService.LogHangfire(user, ControllerContext.RouteData.Values["controller"] + "/" + ControllerContext.RouteData.Values["action"], reqParm));

            return ret;
        }

        [HttpPut("{id}")]
        public async Task<bool> SetGroundless(Guid id)
        {
            return await verifyWorryUseCase.SetGroundless(id);
        }

        [HttpPut("{id}")]
        public async Task<bool> SetApproved(Guid id, string socialSecNum = "")
        {
            var reqParm = await verifyWorryUseCase.SetApproved(id, socialSecNum);

            hangfireManager.EnqueueJob(() => loggerService.LogHangfire(user, ControllerContext.RouteData.Values["controller"] + "/" + ControllerContext.RouteData.Values["action"], reqParm.ToString()));
            return reqParm;
        }

        [HttpGet]
        public async Task<IEnumerable<VerifyWorryMenuItem>> GetMenuItems()
        {
            var ret = (await verifyWorryUseCase.GetMenuItems()).ToList();

            var verifyWorryMenuItems = ret.ToList();
            var reqParmList = verifyWorryMenuItems.Select(x => x.SocialSecData.SocialSecNum != null ? string.Concat("Cpr:", x.SocialSecData.SocialSecNum) : string.Concat("Reported Cpr:", x.ReportedPerson.ReportedCpr)).ToList();

            hangfireManager.EnqueueJob(() => loggerService.LogHangfire(user, ControllerContext.RouteData.Values["controller"] + "/" + ControllerContext.RouteData.Values["action"], reqParmList));

            return verifyWorryMenuItems;
        }


        [HttpGet]
        public async Task<int> GetPendingWorries()
        {
            var count = await verifyWorryUseCase.GetNumberOfPendingWorries();

            return count;
        }

        [HttpPut("{id}")]
        public async Task<bool> Unverify(Guid id)
        {
            return await verifyWorryUseCase.Unverify(id);
        }
    }
}