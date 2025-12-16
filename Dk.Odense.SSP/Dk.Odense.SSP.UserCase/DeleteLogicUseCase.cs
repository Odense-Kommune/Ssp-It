using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.Core;
using Dk.Odense.SSP.UserCase.ViewModel;
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace Dk.Odense.SSP.UserCase
{
    public class DeleteLogicUseCase
    {
        private readonly IPersonService personService;
        private readonly IUnitOfWork unitOfWork;

        int rows = 2;
        int cols = 1;

        public DeleteLogicUseCase(IPersonService personService, IUnitOfWork unitOfWork)
        {
            this.personService = personService;
            this.unitOfWork = unitOfWork;
        }

        public async Task<bool> DeleteLogic()
        {
            var deleteData = await personService.GetPersonsForDeleting(DateTime.Now);
            var allGood = true;


            foreach (var deletePerson in deleteData)
            {
                var res = await personService.Delete(deletePerson.Id);

                if (allGood == false) continue;

                allGood = res;
            }

            await unitOfWork.Commit();

            return allGood;
        }

        public ExportData CreateExcel(DateTime date, string rootPath)
        {
            var path = rootPath + @"\DeletePerson";
            var fileName = $@"DeleteLogic-{date.ToString("dd-MM-yyyy")}.xlsx";
            var file = new FileInfo(Path.Combine(path, fileName));

            MakeDir(path);
            DeleteFiles(path);

            CreateExcelFile(file, date);

            return new ExportData()
            {
                RootFolder = path,
                FileName = fileName,
                FileInfo = file
            };
        }

        private void CreateExcelFile(FileInfo file, DateTime date)
        {
            using var document = new ExcelPackage(file);
            // add a new worksheet to the empty workbook
            var worksheet = document.Workbook.Worksheets.Add("Slettes");
            //First add the headers
            CreateHeaders(worksheet);

            var deleteData = personService.GetPersonsForDeleting(date).Result.ToList();

            var orderedData = deleteData.OrderBy(x => x.Name);

            try
            {
                foreach (var deletePerson in orderedData)
                {
                    SetId(worksheet, deletePerson.Id);
                    SetNavn(worksheet, deletePerson.Name);
                    SetRecidiv(worksheet, deletePerson.WorryCount);
                    SetSenesteInberetning(worksheet, deletePerson.LatestWorryDate);
                    SetSspStopdato(worksheet, deletePerson.SspStopDate);
                    SetSletEfterSspSlut(worksheet, deletePerson.DelteAfterSspEnd);
                    cols = 1;
                    rows++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            var range = worksheet.Cells[1, 1, rows - 1, 6];

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

        private void SetId(ExcelWorksheet worksheet, Guid deletePersonId)
        {
            worksheet.Cells[rows, cols].Value = deletePersonId;
            cols++;
        }

        private void SetNavn(ExcelWorksheet worksheet, string deletePersonName)
        {
            worksheet.Cells[rows, cols].Value = deletePersonName;
            cols++;
        }

        private void SetRecidiv(ExcelWorksheet worksheet, in int deletePersonWorryCount)
        {
            worksheet.Cells[rows, cols].Value = deletePersonWorryCount;
            cols++;
        }

        private void SetSenesteInberetning(ExcelWorksheet worksheet, DateTime? deletePersonLatestWorryDate)
        {
            worksheet.Cells[rows, cols].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;

            worksheet.Cells[rows, cols].Value = deletePersonLatestWorryDate?.Date.ToString("dd-MM-yyyy");

            cols++;
        }

        private void SetSspStopdato(ExcelWorksheet worksheet, DateTime? deletePersonSspStopDate)
        {
            worksheet.Cells[rows, cols].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;

            worksheet.Cells[rows, cols].Value = deletePersonSspStopDate?.Date.ToString("dd-MM-yyyy");

            cols++;
        }

        private void SetSletEfterSspSlut(ExcelWorksheet worksheet, bool? deletePersonDelteAfterSspEnd)
        {
            worksheet.Cells[rows, cols].Value = deletePersonDelteAfterSspEnd;
            cols++;
        }

        private void CreateHeaders(ExcelWorksheet worksheet)
        {
            var headerList = new List<string>()
            {
                "Id",
                "Navn",
                "Recidiv",
                "Seneste Inberetning",
                "SSP Stopdato",
                "Slet efter SSP Slut"
            };



            for (var i = 0; i < headerList.Count; i++)
            {
                worksheet.Cells[1, i + 1].Value = headerList[i];
            }
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
    }
}