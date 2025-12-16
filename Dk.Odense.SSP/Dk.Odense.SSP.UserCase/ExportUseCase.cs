using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.Core;
using Dk.Odense.SSP.Domain.Model;
using Dk.Odense.SSP.UserCase.ViewModel;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using static System.Int64;

namespace Dk.Odense.SSP.UserCase
{
    public class ExportUseCase
    {
        private readonly IAgendaService agendaService;
        private readonly IRobustnessService robustnessService;
        private readonly SchoolDataUseCase schoolDataUseCase;
        private readonly IPersonService personService;
        private readonly CalcWeekNumber calcWeekNumber;

        int rows = 2;
        int cols = 1;

        public ExportUseCase(IAgendaService agendaService, IRobustnessService robustnessService, SchoolDataUseCase schoolDataUseCase, IPersonService personService, CalcWeekNumber calcWeekNumber)
        {
            this.agendaService = agendaService;
            this.robustnessService = robustnessService;
            this.schoolDataUseCase = schoolDataUseCase;
            this.personService = personService;
            this.calcWeekNumber = calcWeekNumber;
        }

        public async Task<ExportData> CreateAgenda(Guid agendaId, string rootPath)
        {
            var path = rootPath + @"\Agendas";
            var fileName = @"Dagsorden " + await GetFileName(agendaId) + ".xlsx";
            var file = new FileInfo(Path.Combine(path, fileName));

            MakeDir(path);
            DeleteFiles(path);

            await UpdateSchoolData(agendaId);

            await CreateExcelFile(file, agendaId);

            return new ExportData()
            {
                RootFolder = path,
                FileName = fileName,
                FileInfo = file
            };
        }

        private async Task UpdateSchoolData(Guid agendaId)
        {
            var personsOnAgenda = await agendaService.GetAllPersonsOnAgenda(agendaId);

            foreach (var person in personsOnAgenda)
            {
                await personService.UpdateSchoolData(person);
            }
        }

        private async Task<string> GetFileName(Guid agendaId)
        {
            var date = (await agendaService.Get(agendaId)).Date;

            var week = calcWeekNumber.GetWeekNumber(date);

            date = StartOfWeek(date, DayOfWeek.Monday);
            return "Uge " + week + " - " + date.ToString("yyyy-MM-dd");
        }

        private static DateTime StartOfWeek(DateTime date, DayOfWeek startOfWeek)
        {
            var diff = (7 + (date.DayOfWeek - startOfWeek)) % 7;
            return date.AddDays(-1 * diff).Date;
        }

        private void MakeDir(string path)
        {
            var directoryInfo = new DirectoryInfo(path);

            if (!directoryInfo.Exists) Directory.CreateDirectory(path);
        }

        private void DeleteFiles(string path)
        {
            var dir = new DirectoryInfo(path);
            foreach (var fileInfo in dir.GetFiles())
            {
                fileInfo.Delete();
            }
        }

        private async Task CreateExcelFile(FileInfo file, Guid agendaId)
        {
            using var document = new ExcelPackage(file);
            // add a new worksheet to the empty workbook
            var worksheet = document.Workbook.Worksheets.Add("Dagsorden");
            //First add the headers
            CreateHeaders(worksheet);

            var agendaData = await agendaService.ExportAgenda(agendaId);

            var orderedAgendaItems = agendaData.AgendaItems.OrderBy(x => x.SortOrder);

            foreach (var agendaItem in orderedAgendaItems)
            {
                var orderedWorries = agendaItem.Worries.OrderBy(x => x.Increment);
                try
                {
                    foreach (var worry in orderedWorries)
                    {
                        SetId(worksheet, worry);
                        SetSource(worksheet, worry);
                        SetName(worksheet, worry);
                        SetAddress(worksheet, worry);
                        SetSocialSecNum(worksheet, worry);
                        SetAge(worksheet, worry);
                        SetSex(worksheet, worry);
                        SetCrimeScene(worksheet, worry);
                        SetWeekNumber(worksheet, worry);
                        SetSspArea(worksheet, worry);
                        await SetSchool(worksheet, worry);
                        await SetLatestRobustnessDate(worksheet, worry);
                        SetRecidiv(worksheet, worry, agendaData.Date);
                        cols = 1;
                        rows++;
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

            }

            var range = worksheet.Cells[1, 1, rows - 1, 14];

            var table = worksheet.Tables.Add(range, "table1");
            table.TableStyle = TableStyles.Medium4;

            worksheet.Cells.AutoFitColumns();

            try
            {
                document.Save(); //Save the workbook.
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void SetRecidiv(ExcelWorksheet worksheet, Worry worry, DateTime agendaDate)
        {
            try
            {
                var date = agendaDate.AddHours(12);

                var count = worry.Person.Worries.Count(x => x.CreatedDate <= date);

                if (count == 0) count = 1;

                worksheet.Cells[rows, cols].Value = count;
            }
            catch
            {
                // ignored
            }

            cols++;
        }

        private void SetId(ExcelWorksheet worksheet, Worry worry)
        {
            worksheet.Cells[rows, cols].Value = worry.Increment;
            cols++;
        }

        private void SetSource(ExcelWorksheet worksheet, Worry worry)
        {
            worksheet.Cells[rows, cols].Value = worry.Reporter.Workplace;
            cols++;
        }

        private void SetName(ExcelWorksheet worksheet, Worry worry)
        {
            worksheet.Cells[rows, cols].Value = worry.Person.Name;
            cols++;
        }

        private void SetAddress(ExcelWorksheet worksheet, Worry worry)
        {
            worksheet.Cells[rows, cols].Value = worry.Person.Address;
            cols++;
        }

        private void SetSocialSecNum(ExcelWorksheet worksheet, Worry worry)
        {
            worksheet.Cells[rows, cols].Value = worry.Person.SocialSecNum;
            cols++;
        }

        private void SetAge(ExcelWorksheet worksheet, Worry worry)
        {
            var bornDay = Int32.Parse(worry.Person.Birthday.Substring(6, 2));
            var bornMonth = Int32.Parse(worry.Person.Birthday.Substring(4, 2));
            var bornYear = Int32.Parse(worry.Person.Birthday.Substring(0, 4));

            var born = new DateTime(bornYear, bornMonth, bornDay);

            var span = DateTime.Now.Date - born;

            var age = (new DateTime(1, 1, 1) + span).Year - 1;

            worksheet.Cells[rows, cols].Value = age;
            cols++;
        }

        private void SetSex(ExcelWorksheet worksheet, Worry worry)
        {
            var sex = Parse(worry.Person.SocialSecNum) % 2 != 0 ? "M" : "K";

            worksheet.Cells[rows, cols].Value = sex;
            cols++;
        }

        private void SetCrimeScene(ExcelWorksheet worksheet, Worry worry)
        {
            worksheet.Cells[rows, cols].Value = worry.CrimeScene;
            cols++;
        }

        private void SetWeekNumber(ExcelWorksheet worksheet, Worry worry)
        {
            worksheet.Cells[rows, cols].Value = calcWeekNumber.GetWeekNumber(worry.CreatedDate
                );
            cols++;
        }

        private void SetSspArea(ExcelWorksheet worksheet, Worry worry)
        {
            worksheet.Cells[rows, cols].Value = worry.Person.SspArea?.Value;
            cols++;
        }

        private async Task SetSchool(ExcelWorksheet worksheet, Worry worry)
        {
            var schoolData = await schoolDataUseCase.GetSchoolDataByPersonId(worry.Person.Id);

            worksheet.Cells[rows, cols].Value = schoolData != null ? schoolData.Name : "";
            cols++;
            worksheet.Cells[rows, cols].Value = schoolData != null ? schoolData.DateFirst.Date + " - " + schoolData.DateLast.Date : "";
            cols++;
        }

        private async Task SetLatestRobustnessDate(ExcelWorksheet worksheet, Worry worry)
        {
            worksheet.Cells[rows, cols].Value = worry.Person_Id == null ? "" : (await robustnessService.GetPrevious((Guid)worry.Person_Id, int.MaxValue))?.CreatedDate.ToString("yyyy-MM-dd");
            cols++;
        }

        private void CreateHeaders(ExcelWorksheet worksheet)
        {
            var headerList = new List<string>()
            {
                "ID",
                "Afsender",
                "Navn",
                "Bopæl",
                "Cpr",
                "Alder",
                "Køn",
                "G-sted",
                "Uge nr.",
                "Sagsområde",
                "Skoleplacering",
                "Skole periode",
                "Seneste Robusthedsskema",
                "Recidiv"
            };

            for (var i = 0; i < headerList.Count; i++)
            {
                worksheet.Cells[1, i + 1].Value = headerList[i];
            }
        }
    }
}
