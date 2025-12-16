using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.Core;
using Dk.Odense.SSP.Domain.Model;
using Dk.Odense.SSP.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dk.Odense.SSP.Web.Controllers.Api.NonViewModel
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Policy = "SSP")]
    public class NoteSharedController : BaseController<NoteShared>
    {
        private readonly INoteSharedService service;
        private readonly HangfireManager hangfireManager;
        private readonly ILoggerService loggerService;
        private readonly string user;

        public NoteSharedController(INoteSharedService service, IUnitOfWork unitOfWork, ILoggerService loggerService, HangfireManager hangfireManager, IHttpContextAccessor contextAccessor) : base(service, unitOfWork)
        {
            this.service = service;
            this.loggerService = loggerService;
            this.hangfireManager = hangfireManager;

            var userContext = contextAccessor.HttpContext?.User ?? null;
            user = userContext?.Claims.FirstOrDefault(c => c.Type == "preferred_username")?.Value ?? "Hangfire";
            user = user.Replace("@odense.dk", "");
        }

        [HttpGet]
        public async Task<IEnumerable<NoteShared>> GetNoteByPerson(Guid personId)
        {
            var ret = await service.GetNoteByPerson(personId);

            hangfireManager.EnqueueJob(() => loggerService.LogHangfire(user, ControllerContext.RouteData.Values["controller"] + "/" + ControllerContext.RouteData.Values["action"], string.Concat("PersonId:", personId)));

            return ret;
        }

        [HttpPost]
        public override async Task<NoteShared> Post(NoteShared value)
        {
            value.Reporter = user;

            if (value.Value == null) return value;

            if (await service.Get(value.Id) == null) return await base.Post(value);

            return await base.Put(value);
        }
    }
}
