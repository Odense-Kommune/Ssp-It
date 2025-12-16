using System;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;
using Dk.Odense.SSP.Core.Configuration;
using Dk.Odense.SSP.UserCase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Dk.Odense.SSP.Web.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DeleteLogicController : ControllerBase
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly DeleteLogicUseCase deleteLogicUseCase;
        private readonly MailConfig mailConfig;

        public DeleteLogicController(IWebHostEnvironment webHostEnvironment, DeleteLogicUseCase deleteLogicUseCase, IOptions<MailConfig> mailConfig)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.deleteLogicUseCase = deleteLogicUseCase;
            this.mailConfig = mailConfig.Value;
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> ManualDelete()
        {
            var res = await deleteLogicUseCase.DeleteLogic();

            return Ok(res);
        }

        [HttpGet]
        [Authorize]
        public bool SendMail()
        {
            try
            {
                var message = new MailMessage { From = new MailAddress(mailConfig.FromMail) };

                foreach (var mailReceiver in mailConfig.Receivers)
                {
                    message.To.Add(new MailAddress(mailReceiver));
                }

                message.Subject = $"SSP Personer som slettes 1-{DateTime.Now.AddMonths(1).Month}-{DateTime.Now.Year}";
                message.Body = "T.o";

                var exportData = deleteLogicUseCase.CreateExcel(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)), webHostEnvironment.WebRootPath);

                var attachmentFile = PhysicalFile(Path.Combine(exportData.RootFolder, exportData.FileName), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

                message.Attachments.Add(new Attachment(attachmentFile.FileName));

                var smtp = new SmtpClient(mailConfig.Smtp, mailConfig.SmtpPort);

                smtp.Send(message);

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        [Authorize]
        public PhysicalFileResult DeleteLogicExcel(DateTime date)
        {
            try
            {
                var exportData = deleteLogicUseCase.CreateExcel(date, webHostEnvironment.WebRootPath);

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
