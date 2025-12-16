using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.Core;
using Dk.Odense.SSP.Domain.Model;
using Dk.Odense.SSP.UserCase.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml;
using Enum = Dk.Odense.SSP.Core.Enum;
using Microsoft.AspNetCore.Http;

namespace Dk.Odense.SSP.UserCase
{
    public class UploadUseCase
    {
        private readonly IReportedPersonService reportedPersonService;
        private readonly IWorryService worryService;
        private readonly ISourceService sourceService;
        private readonly IUnitOfWork unitOfWork;

        public UploadUseCase(IReportedPersonService reportedPersonService, IWorryService worryService,
            IUnitOfWork unitOfWork, ISourceService sourceService)
        {
            this.reportedPersonService = reportedPersonService;
            this.worryService = worryService;
            this.unitOfWork = unitOfWork;
            this.sourceService = sourceService;
        }

        public async Task<bool> PersistExcelUpload(IEnumerable<ExcelUploadResult> excelUpload)
        {
            foreach (var line in excelUpload)
            {
                await reportedPersonService.Create(line.ReportedPerson);
                line.Worry.ReportedPerson_Id = line.ReportedPerson.Id;
                var source = await sourceService.Get((Guid)line.Worry.Source_Id);
                AddReporter(line.Worry, source);
                line.Worry.Concern = line.Concern;
                await worryService.Create(line.Worry);
            }

            await unitOfWork.Commit();
            return true;
        }


        private void AddReporter(Worry worry, Source source)
        {
            worry.Reporter = new Reporter
            {
                Workplace = source.Value
            };
        }

        public async Task<IEnumerable<ExcelUploadResult>> UploadExcelDoc(IFormFile file)
        {
            var result = new List<ExcelUploadResult>();
            try
            {
                var fileName = $"./Excel-temp.xlsx";
                File.Create(fileName).Close();
                var excelFileStream = new FileStream(fileName, FileMode.Open);

                await file.CopyToAsync(excelFileStream);

                var excel = new ExcelPackage(excelFileStream);

                excelFileStream.Close();

                var worksheet = excel.Workbook.Worksheets[0];
                int? nameCol = null;
                int? adressCol = null;
                int? cprCol = null;
                int? crimeCol = null;
                int? dateCol = null;
                int? concernCol = null;

                for (var i = 1; i <= 7; i++)
                {
                    try
                    {
                        string firstRow = worksheet.Cells[1, i].Value.ToString();

                        switch (firstRow.ToLower())
                        {
                            case "navn":
                                nameCol = i;
                                break;
                            case "adresse":
                                adressCol = i;
                                break;
                            case "cpr":
                                cprCol = i;
                                break;
                            case "gerningssted":
                                crimeCol = i;
                                break;
                            case "dato":
                                dateCol = i;
                                break;
                            case "bekymring":
                                concernCol = i;
                                break;
                            default:
                                Console.WriteLine("Nothing");
                                break;
                        }
                    }
                    catch
                    {
                        break;
                        //ignored
                    }
                }

                var errors = CheckMissingMandatoryCols(nameCol, adressCol, cprCol, crimeCol);

                if (errors.Any())
                {
                    var res = new List<ExcelUploadResult> { new ExcelUploadResult() { Errors = errors } };
                    return res;
                }

                var endOfDocument = false;
                var concurrentEmptyLines = 0;
                var errorlines = new List<int>();

                for (var i = 2; !endOfDocument; i++)
                {
                    try
                    {
                        var name = worksheet.Cells[i, (int)nameCol].Text;
                        var cpr = worksheet.Cells[i, (int)cprCol].Text;
                        var crime = worksheet.Cells[i, (int)crimeCol].Text;
                        var address = worksheet.Cells[i, (int)adressCol]?.Text ?? "";

                        string date = null;
                        string concern = null;
                        if (dateCol != null) date = ParseDate(worksheet.Cells[i, (int)dateCol].Value?.ToString());
                        if (concernCol != null) concern = worksheet.Cells[i, (int)concernCol].Value.ToString();

                        if (cpr == null || (int.TryParse(cpr, out var x) != true && cpr.Length != 10))
                        {
                            cpr = $"Ikke gyldigt cpr: {cpr}";
                        }

                        var newResult = new ExcelUploadResult()
                        {
                            ReportedPerson = new ReportedPerson()
                            {
                                ReportedName = name,
                                ReportedAdress = address,
                                ReportedCpr = cpr
                            },
                            Worry = new Worry()
                            {
                                CrimeScene = crime,
                                CreatedDate = date != null ? DateTime.Parse(date) : DateTime.Today,
                            }
                        };

                        if (concern != null)
                        {
                            newResult.Concern = new Concern()
                            {
                                CrimeConcern = concern,
                                NotifyConcern = Enum.Answer.DontKnow,
                                ReportedToPolice = Enum.Answer.DontKnow
                            };
                        }

                        result.Add(newResult);

                        concurrentEmptyLines = 0;
                    }
                    catch (Exception e)
                    {
                        concurrentEmptyLines++;
                        errorlines.Add(i);


                        if (concurrentEmptyLines != 3) continue;

                        var errorLineList = new List<string>();

                        if (errorlines.Count - 3 != 0)
                        {
                            for (var j = 0; j < errorlines.Count - 3; j++)
                            {
                                errorLineList.Add($"Linje {errorlines[j]} mangler data");
                            }

                            result[0].Errors = errorLineList;
                        }

                        endOfDocument = true;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return result;
        }

        private static List<string> CheckMissingMandatoryCols(int? nameCol, int? adressCol, int? cprCol, int? crimeCol)
        {
            var missingCols = new List<string>();

            if (nameCol == null)
            {
                missingCols.Add("Kolonne navn mangler");
            }

            if (cprCol == null)
            {
                missingCols.Add("Kolonne cpr mangler");
            }

            if (crimeCol == null)
            {
                missingCols.Add("Kolonne gerningssted mangler");
            }

            return missingCols;
        }

        private static string ParseDate(string dateString)
        {
            try
            {
                return dateString == null ? DateTime.Today.ToString() : DateTime.Parse(dateString).ToString();
            }
            catch
            {
                // ignored
            }

            var days = int.Parse(dateString);

            return DateTime.MinValue.AddDays(days).ToString();
        }
    }
}
