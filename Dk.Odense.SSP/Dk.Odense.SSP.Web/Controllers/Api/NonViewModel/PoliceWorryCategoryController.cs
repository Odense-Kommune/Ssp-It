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
    public class PoliceWorryCategoryController : BaseController<PoliceWorryCategory>
    {
        private readonly IPoliceWorryCategoryService policeWorryCategoryService;
        public PoliceWorryCategoryController(IPoliceWorryCategoryService policeWorryCategoryService, IUnitOfWork unitOfWork) : base(policeWorryCategoryService, unitOfWork)
        {
            this.policeWorryCategoryService = policeWorryCategoryService;
        }

        [HttpGet]
        public async Task<IEnumerable<PoliceWorryCategory>> GetValidList()
        {
            return await policeWorryCategoryService.GetValidList();
        }
    }
}