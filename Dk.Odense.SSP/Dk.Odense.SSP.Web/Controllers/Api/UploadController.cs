using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dk.Odense.SSP.Logger;
using Dk.Odense.SSP.UserCase;
using Dk.Odense.SSP.UserCase.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dk.Odense.SSP.Web.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Policy = "SSP")]
    public class UploadController : ControllerBase
    {
        private readonly UploadUseCase uploadUseCase;
        private readonly ILoggerService logger;
        private readonly HangfireManager hangfireManager;
        private readonly string user;

        public UploadController(ILoggerService logger, HangfireManager hangfireManager,
            UploadUseCase uploadUseCase, IHttpContextAccessor contextAccessor)
        {
            this.logger = logger;
            this.hangfireManager = hangfireManager;
            this.uploadUseCase = uploadUseCase;

            var userContext = contextAccessor.HttpContext?.User ?? null;
            user = userContext?.Claims.FirstOrDefault(c => c.Type == "preferred_username")?.Value ?? "Hangfire";
            user = user.Replace("@odense.dk", "");
        }

        [HttpPost]
        public async Task<bool> ExcelDataToWorries([FromBody] IEnumerable<ExcelUploadResult> worries)
        {
            hangfireManager.EnqueueJob(() => logger.LogHangfire(user,
                ControllerContext.RouteData.Values["controller"] + "/" + ControllerContext.RouteData.Values["action"],
                worries.Select(s =>
                    string.Concat(
                        "Reported CPR: ", s.ReportedPerson.ReportedCpr,
                        " Reported Name: ", s.ReportedPerson.ReportedName,
                        " Reported Address: ", s.ReportedPerson.ReportedAdress)).ToList()));

            var result = await uploadUseCase.PersistExcelUpload(worries);
            if (result) hangfireManager.EnqueueVerify();
            return result;
        }

        [HttpPost]
        public async Task<JsonResult> UploadExcel(IFormFile file)
        {
            var result = await uploadUseCase.UploadExcelDoc(file);

            return new JsonResult(result.ToArray());
        }

        [HttpPost]
        public async Task<IActionResult> ManualVerify()
        {
            try
            {
                hangfireManager.EnqueueVerify();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}