using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dk.Odense.SSP.Gdf.Model;
using Dk.Odense.SSP.Gdf.Model.Interfaces;
using Dk.Odense.SSP.Gdf.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Dk.Odense.SSP.Web.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Policy = "SSP")]
    public class GdfController : ControllerBase
    {
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly IGdfService<AvaDev> avaService;
        private readonly IGdfService<AvaProd> avaServiceProd;
        private readonly IGdfService<RobusthedDev> robustService;
        private readonly IGdfService<RobusthedProd> robustServiceProd;

        public GdfController(IWebHostEnvironment hostingEnvironment, IGdfService<AvaDev> avaService, IGdfService<AvaProd> avaServiceProd, IGdfService<RobusthedDev> robustService, IGdfService<RobusthedProd> robustServiceProd)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.avaService = avaService;
            this.avaServiceProd = avaServiceProd;
            this.robustService = robustService;
            this.robustServiceProd = robustServiceProd;
        }

        [HttpGet]
        public async Task<bool> LoadAvaData()
        {
            return await avaService.LoadData();
        }

        [HttpGet]
        public async Task<bool> LoadRobustData()
        {
            return await robustService.LoadData();
        }

        [HttpGet]
        public IEnumerable<IAva> GetAvas()
        {
            if (hostingEnvironment.IsDevelopment()) return avaService.List().ToList();

            return avaServiceProd.List().ToList();
        }

        [HttpGet]
        public IEnumerable<IRobusthed> GetRobustheds()
        {
            if (hostingEnvironment.IsDevelopment()) return robustService.List().ToList();

            return robustServiceProd.List().ToList();
        }
    }
}