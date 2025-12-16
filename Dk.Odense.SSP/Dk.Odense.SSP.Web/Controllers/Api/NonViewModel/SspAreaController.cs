using System.Collections.Generic;
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
    public class SspAreaController : BaseController<SspArea>
    {
        private readonly ISspAreaService service;
        public SspAreaController(ISspAreaService service, IUnitOfWork unitOfWork) : base(service, unitOfWork)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<SspArea>> GetValidList()
        {
            return await service.GetValidList();
        }
    }
}