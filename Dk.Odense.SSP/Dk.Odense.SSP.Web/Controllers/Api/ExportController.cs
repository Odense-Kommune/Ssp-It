using System;
using System.IO;
using System.Threading.Tasks;
using Dk.Odense.SSP.UserCase;
using Dk.Odense.SSP.UserCase.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Dk.Odense.SSP.Web.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Policy = "SSP")]
    public class ExportController : ControllerBase
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ExportUseCase exportUseCase;
        private readonly ExportAgendaItemUseCase exportAgendaItemUseCase;
        private readonly GroupingUseCase groupingUseCase;

        int rows = 2;
        int cols = 1;

        public ExportController(ExportUseCase exportUseCase, IWebHostEnvironment webHostEnvironment, ExportAgendaItemUseCase exportAgendaItemUseCase, GroupingUseCase groupingUseCase)
        {
            this.exportUseCase = exportUseCase;
            this.webHostEnvironment = webHostEnvironment;
            this.exportAgendaItemUseCase = exportAgendaItemUseCase;
            this.groupingUseCase = groupingUseCase;
        }

        [HttpGet]
        public async Task<PhysicalFileResult> GetAgenda(Guid agendaId)
        {
            try
            {
                var exportData = await exportUseCase.CreateAgenda(agendaId, webHostEnvironment.WebRootPath);

                var result = PhysicalFile(Path.Combine(exportData.RootFolder, exportData.FileName), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

                Response.Headers["Content-Disposition"] = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = exportData.FileInfo.Name
                }.ToString();

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        [HttpGet]
        public async Task<PhysicalFileResult> GetAgendaItem(Guid agendaItemId)
        {
            try
            {
                var exportData = await exportAgendaItemUseCase.CreateWordDocument(agendaItemId, webHostEnvironment.WebRootPath);

                var result = PhysicalFile(Path.Combine(exportData.RootFolder, exportData.FileName), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

                Response.Headers["Content-Disposition"] = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = exportData.FileInfo.Name
                }.ToString();

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        [HttpPost]
        public async Task<PhysicalFileResult> GetCrossRef(ExportCrossRefDto exportCrossRefDto)
        {
            try
            {
                var exportData = await groupingUseCase.GetGroupingExport(exportCrossRefDto.Id, exportCrossRefDto.SelectedPersons, exportCrossRefDto.SelectedCategorizations, exportCrossRefDto.GroupingsType, webHostEnvironment.WebRootPath);
                
                var result = PhysicalFile(Path.Combine(exportData.RootFolder, exportData.FileName), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

                Response.Headers["Content-Disposition"] = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = exportData.FileInfo.Name
                }.ToString();

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}