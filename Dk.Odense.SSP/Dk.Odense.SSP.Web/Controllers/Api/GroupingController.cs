using System;
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
    public class GroupingController : ControllerBase
    {
        private readonly GroupingUseCase groupingUseCase;
        private readonly HangfireManager hangfireManager;
        private readonly ILoggerService loggerService;
        private readonly string user;

        public GroupingController(GroupingUseCase groupingUseCase, ILoggerService loggerService, HangfireManager hangfireManager, IHttpContextAccessor contextAccessor)
        {
            this.groupingUseCase = groupingUseCase;
            this.loggerService = loggerService;
            this.hangfireManager = hangfireManager;

            var userContext = contextAccessor.HttpContext?.User ?? null;
            user = userContext?.Claims.FirstOrDefault(c => c.Type == "preferred_username")?.Value ?? "Hangfire";
            user = user.Replace("@odense.dk", "");
        }

        [HttpGet]
        public async Task<IEnumerable<Grouping>> SearchGroupings(string query)
        {
            var res = await groupingUseCase.SearchGroupings(query);

            return res;
        }

        [HttpGet]
        public async Task<IEnumerable<Grouping>> List()
        {
            return await groupingUseCase.GetGroupingList();
        }

        [HttpPost]
        public async Task<Grouping> Post(Grouping group)
        {
            group.Type = "grouping";
            return await groupingUseCase.AddGroup(group);
        }

        [HttpPost]
        public async Task<Grouping> PostPsu(Grouping group)
        {
            group.Type = "psu";
            return await groupingUseCase.AddGroup(group);
        }

        [HttpPost]
        public async Task<PersonGrouping> AddPersonToGroup([FromBody] PersonGrouping grouping)
        {
            return await groupingUseCase.CreatePersonGrouping(grouping);
        }

        [HttpPost]
        public async Task<PersonGrouping> SetClassification(Guid personId, Guid groupId, Guid classificationId)
        {
            return await groupingUseCase.SetClassification(personId, groupId, classificationId);
        }

        [HttpGet]
        public async Task<IEnumerable<PersonGroups>> GetPeopleForGroup(Guid groupId)
        {
            var ret = await groupingUseCase.GetPeopleForGroup(groupId);

            hangfireManager.EnqueueJob(() => loggerService.LogHangfire(user, ControllerContext.RouteData.Values["controller"] + "/" + ControllerContext.RouteData.Values["action"], ret.Select(x => string.Concat("Cpr:", x.cpr)).ToList()));

            return ret;
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFromGroup(Guid personId, Guid groupId)
        {
            var res = await groupingUseCase.DeleteFromGroup(personId, groupId);
            if (res == false) return BadRequest();
            return Ok(res);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await groupingUseCase.DeleteGroup(id);
            //Response.StatusCode = 204;
            return Ok();
        }

        [HttpGet]
        public async Task<IEnumerable<MenuGrouping>> MenuGroupingList()
        {
            return await groupingUseCase.MenuGroupingList();
        }

        [HttpGet]
        public async Task<IEnumerable<MenuGrouping>> MenuPsuGroupingList()
        {
            return await groupingUseCase.MenuPsuGroupingList();
        }

        [HttpPut]
        public async Task<Grouping> Put(Grouping grouping)
        {
            return await groupingUseCase.Update(grouping);
        }

        [HttpGet]
        public async Task<IEnumerable<GroupingStatsDto>> GetGroupingStats(Guid id)
        {
            return await groupingUseCase.GetGroupingStats(id);
        }
    }
}