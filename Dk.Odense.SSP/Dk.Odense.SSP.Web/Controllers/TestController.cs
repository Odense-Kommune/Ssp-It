using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dk.Odense.SSP.Domain.Model;
using Dk.Odense.SSP.Sbsys.Interfaces;
using Dk.Odense.SSP.Xflow.UseCase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Robustness = Dk.Odense.SSP.Domain.Model.Robustness;

namespace Dk.Odense.SSP.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ISbsysCaseService sbsysCaseService;
        private readonly XFlowRobustnessUseCase _xFlowRobustnessUseCase;
        private readonly XFlowWorryUseCase _xFlowWorryUseCase;

        public TestController(
            ISbsysCaseService sbsysCaseService,
            XFlowRobustnessUseCase robustnessXFlowUseCase,
            XFlowWorryUseCase avaXFlowUseCase)
        {
            this.sbsysCaseService = sbsysCaseService;
            _xFlowRobustnessUseCase = robustnessXFlowUseCase;
            _xFlowWorryUseCase = avaXFlowUseCase;
        }

        [HttpGet]
        public async Task<IActionResult> Test()
        {

            var r = HttpContext.User.Claims.ToList();

            var user = User.Identity;

            return Ok();
        }

        [HttpPost]
        public async Task<IEnumerable<Robustness>> MapNewRobustnesses()
        {
            try
            {
                return await _xFlowRobustnessUseCase.GetNewRobustnessesXFlow();
            }
            catch(Exception) { 
                
                return null;
            }
        }

        [HttpPost]
        public async Task<IEnumerable<XFlowRobustness>> GetNewRobustnessForms()
        {
            try
            {
                return await _xFlowRobustnessUseCase.GetNewRobustnessForms();
            }
            catch(Exception)
            {
                return null;
            }
        }

        [HttpPost]
        public async Task<IEnumerable<XFlowWorry>> GetNewWorryForms()
        {
            try
            {
                return await _xFlowWorryUseCase.GetNewWorryForms();
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPost]
        public async Task<IEnumerable<Worry>> MapNewWorries()
        {
            try
            {
                return await _xFlowWorryUseCase.GetNewWorriesXFlow();
            }
            catch(Exception)
            {   
                return null;
            }
        }
    }
}
