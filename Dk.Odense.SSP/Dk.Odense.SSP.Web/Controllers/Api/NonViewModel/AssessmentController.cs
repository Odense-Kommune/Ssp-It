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
    public class AssessmentController : BaseController<Assessment>
    {
        public AssessmentController(IAssessmentService service, IUnitOfWork unitOfWork) : base(service, unitOfWork)
        {
        }
    }
}