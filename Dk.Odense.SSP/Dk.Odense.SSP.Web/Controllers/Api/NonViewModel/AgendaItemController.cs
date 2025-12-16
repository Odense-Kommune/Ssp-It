using System;
using System.Threading.Tasks;
using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.Core;
using Dk.Odense.SSP.Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dk.Odense.SSP.Web.Controllers.Api.NonViewModel
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Policy = "SSP")]
    public class AgendaItemController : BaseController<AgendaItem>
    {
        private readonly IAgendaItemService agendaItemService;
        private readonly IUnitOfWork unitOfWork;

        public AgendaItemController(IAgendaItemService service, IUnitOfWork unitOfWork) : base(service, unitOfWork)
        {
            agendaItemService = service;
            this.unitOfWork = unitOfWork;
        }

        [HttpPut]
        public async Task<bool> SetCategorization(Guid id, Guid categorizationId)
        {
            var ret = await agendaItemService.SetCategorization(id, categorizationId);
            await unitOfWork.Commit();
            return ret;
        }
    }
}