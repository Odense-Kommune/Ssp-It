using System.Collections.Generic;
using System.Threading.Tasks;
using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.Core;
using Dk.Odense.SSP.Domain.Model;
using Dk.Odense.SSP.Web.Controllers.Api.NonViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dk.Odense.SSP.Web.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Policy = "SSP")]
    public class SourceController : BaseController<Source>
    {
        private readonly ISourceService sourceService;

        public SourceController(ISourceService sourceService, IUnitOfWork unitOfWork) : base(sourceService, unitOfWork)
        {
            this.sourceService = sourceService;
        }

        [HttpGet]
        public async Task<IEnumerable<Source>> GetValidList()
        {
            return await sourceService.GetValidList();
        }
    }
}