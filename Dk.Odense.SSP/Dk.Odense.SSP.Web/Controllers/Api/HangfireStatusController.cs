using System;
using System.Collections.Generic;
using System.Linq;
using Dk.Odense.SSP.UserCase.ViewModel;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace Dk.Odense.SSP.Web.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HangfireStatusController : ControllerBase
    {
        private readonly Hangfire.JobStorage hangFireJobStorage;

        public HangfireStatusController(JobStorage hangFireJobStorage)
        {
            this.hangFireJobStorage = hangFireJobStorage;
        }

        [HttpGet]
        public bool UpdateSspAreaStatus()
        {
            return hangFireJobStorage.GetMonitoringApi().ProcessingJobs(0, 3).Any(x => x.Value.Job.Method.Name == "UpdateSspArea");
        }

        [HttpGet]
        public IEnumerable<ProcessingJobs> ProcessingJobs()
        {
            return hangFireJobStorage.GetMonitoringApi().ProcessingJobs(0, 3).Select(x => new ProcessingJobs()
            {
                JobName = x.Value.Job.Method.Name,
                RunningForMin = TimeRunning(x.Value.StartedAt)
            });
        }

        private string TimeRunning(DateTime? startedAt)
        {
            return startedAt == null ? "" : (DateTime.Now - (DateTime)startedAt).Minutes.ToString();
        }
    }
}