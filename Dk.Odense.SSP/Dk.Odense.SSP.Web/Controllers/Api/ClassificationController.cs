using Dk.Odense.SSP.Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.Web.Controllers.Api.NonViewModel;
using Dk.Odense.SSP.Core;

namespace Dk.Odense.SSP.Web.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Policy = "SSP")]
    public class ClassificationController : BaseController<Classification>
    {
        private readonly IClassificationService service;

        public ClassificationController(IClassificationService service, IUnitOfWork unitOfWork) : base(service, unitOfWork)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<Classification>> GetValidList()
        {
            return await service.GetValidList();
        }
    }
}