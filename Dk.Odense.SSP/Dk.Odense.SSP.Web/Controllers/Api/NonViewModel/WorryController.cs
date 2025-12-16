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
    public class WorryController : BaseController<Worry>
    {
        private readonly IWorryService service;
        private readonly IUnitOfWork unitOfWork;

        public WorryController(IWorryService service, IUnitOfWork unitOfWork) : base(service, unitOfWork)
        {
            this.service = service;
            this.unitOfWork = unitOfWork;
        }

        [HttpPut]
        public async Task<bool> SetPoliceWorryRole(Guid id, Guid policeWorryRoleId)
        {
            var ret = await service.SetPoliceWorryRole(id, policeWorryRoleId);
            await unitOfWork.Commit();
            return ret;
        }

        [HttpPut]
        public async Task<bool> SetPoliceWorryCategory(Guid id, Guid policeWorryCategoryId)
        {
            var ret = await service.SetPoliceWorryCategory(id, policeWorryCategoryId);
            await unitOfWork.Commit();
            return ret;
        }
    }
}