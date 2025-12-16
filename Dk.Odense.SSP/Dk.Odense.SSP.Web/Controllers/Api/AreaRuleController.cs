using System;
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
    public class AreaRuleController : BaseController<AreaRule>
    {
        private readonly IAreaRuleService service;
        public AreaRuleController(IAreaRuleService service, IUnitOfWork unitOfWork) : base(service, unitOfWork)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<AreaRule>> Test()
        {
            return await service.List();
        }


        [AcceptVerbs("DELETE")]
        public override Task<bool> Delete(Guid id)
        {
            return base.Delete(id);
        }

    }
}