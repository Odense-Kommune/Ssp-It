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
    public class RobustnessController : BaseController<Robustness>
    {
        private readonly IRobustnessService service;
        public RobustnessController(IRobustnessService service, IUnitOfWork unitOfWork) : base(service, unitOfWork)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<Robustness> GetNextByPerson(Guid personId, int increment = 0)
        {
            var ret = await service.GetNext(personId, increment);

            return ret;
        }

        [HttpGet]
        public async Task<Robustness> GetPreviousByPerson(Guid personId, int increment = 0)
        {
            var ret = await service.GetPrevious(personId, increment);

            return ret;
        }
    }
}