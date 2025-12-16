using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Dk.Odense.SSP.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnviromentController : ControllerBase
    {
        private readonly IWebHostEnvironment env;

        public EnviromentController(IWebHostEnvironment env)
        {
            this.env = env;
        }

        [HttpGet]
        public IActionResult GetEnviroment()
        {
            var dict = new Dictionary<string, string>();

            dict.Add("Development", (env.EnvironmentName == "Development").ToString());
            dict.Add("Staging", (env.EnvironmentName == "Staging").ToString());
            dict.Add("Preproduction", (env.EnvironmentName == "Preproduction").ToString());
            dict.Add("Production", (env.EnvironmentName == "Production").ToString());

            return Ok(dict);
        }
    }
}
