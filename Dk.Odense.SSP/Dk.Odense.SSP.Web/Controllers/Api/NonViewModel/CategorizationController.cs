using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.Core;
using Dk.Odense.SSP.Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dk.Odense.SSP.Web.Controllers.Api.NonViewModel
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Policy = "SSP")]
    public class CategorizationController : BaseController<Categorization>
    {

        private readonly ICategorizationService categorizationService;
        public CategorizationController(ICategorizationService categorizationService, IUnitOfWork unitOfWork) : base(categorizationService, unitOfWork)
        {
            this.categorizationService = categorizationService;
        }

        [HttpGet]
        public async Task<IEnumerable<Categorization>> GetValidList()
        {
            return await categorizationService.GetValidList();
        }
    }
}