using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.Core;
using Dk.Odense.SSP.Domain.Model;
using Dk.Odense.SSP.UserCase.ViewModel;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using Person = Dk.Odense.SSP.Domain.Model.Person;

namespace Dk.Odense.SSP.UserCase
{
    public class GroupingUseCase
    {
        private readonly IGroupingService groupingService;
        private readonly IPersonGroupingService personGroupingService;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICategorizationService categorizationService;
        private readonly IPersonService personService;

        int rows = 2;
        int cols = 1;
        int maxCols = 0;
        int maxRows = 0;
        public GroupingUseCase(IGroupingService groupingService, IUnitOfWork unitOfWork, IPersonGroupingService personGroupingService, ICategorizationService categorizationService, IPersonService personService)
        {
            this.groupingService = groupingService;
            this.personGroupingService = personGroupingService;
            this.categorizationService = categorizationService;
            this.personService = personService;
            this.unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Grouping>> GetGroupingList()
        {
            return await groupingService.List();
        }

        public async Task<Grouping> AddGroup(Grouping group)
        {
            var grouping = await groupingService.Create(group);
            await unitOfWork.Commit();
            return grouping;
        }

        public async Task<PersonGrouping> CreatePersonGrouping(PersonGrouping grouping)
        {
            if (await personGroupingService.AnyPersonGroupingFromPersonIdGroupId(grouping.Person_Id, grouping.Grouping_Id)) return grouping;

            var group = await personGroupingService.Create(grouping);
            await unitOfWork.Commit();
            return group;
        }

        public async Task<bool> DeleteFromGroup(Guid personId, Guid groupId)
        {
            var res = await personGroupingService.DeleteGroupByPersonAndGroupId(personId, groupId);
            await unitOfWork.Commit();

            return res;
        }

        public async Task DeleteGroup(Guid groupId)
        {
            var groups = await personGroupingService.GetFromGroupingId(groupId);

            foreach (var group in groups)
            {
                await personGroupingService.Delete(group.Id);
            }
            await groupingService.Delete(groupId);
            await unitOfWork.Commit();
        }

        public async Task<IEnumerable<MenuGrouping>> MenuGroupingList()
        {
            var res = await groupingService.GetWithPersonGrouping();

            return res.Select(x => new MenuGrouping
            {
                Id = x.Id,
                Value = x.Value,
                PersonCount = x.PersonGroupings.Count
            });
        }

        public async Task<IEnumerable<MenuGrouping>> MenuPsuGroupingList()
        {
            var res = await groupingService.GetPsuWithPersonGrouping();

            return res.Select(x => new MenuGrouping
            {
                Id = x.Id,
                Value = x.Value,
                PersonCount = x.PersonGroupings.Count
            });
        }

        public async Task<IEnumerable<PersonGroups>> GetPeopleForGroup(Guid groupId)
        {
            var persons = await personGroupingService.GetPersonsInGroup(groupId);

            var r = persons.Select(x => new PersonGroups()
            {
                Name = x.Person.Name,
                cpr = x.Person.SocialSecNum,
                Person_Id = x.Person_Id,
                Classification_Id = x.Classification_Id ?? Guid.Empty,
                groups = x.Person.PersonGroupings.Select(y => new Grouping()
                {
                    Id = y.Grouping_Id,
                    Value = y.Grouping.Value,
                    Type = y.Grouping.Type,
                    ClassificationName = y?.Classification?.Value
                }).ToList()
            });

            return r;
        }

        public async Task<IEnumerable<Grouping>> SearchGroupings(string query)
        {
            var groups = await groupingService.List();
            var searchGroupings = groups.ToList();
            var res = searchGroupings.Where(x => x.Value.ToLower().Contains(query));

            return res;
        }

        public async Task<Grouping> Update(Grouping grouping)
        {
            var res = groupingService.Update(grouping);

            await unitOfWork.Commit();

            return res;
        }

        public async Task<PersonGrouping> SetClassification(Guid personId, Guid groupId, Guid? classificationId)
        {
            if (classificationId == Guid.Empty)
            {
                classificationId = null;
            }

            var dbEntity = await personGroupingService.GetFromPersonGroup(personId, groupId);

            dbEntity.Classification_Id = classificationId;

            var res = personGroupingService.Update(dbEntity);

            await unitOfWork.Commit();

            return res;
        }

        public async Task<IEnumerable<GroupingStatsDto>> GetGroupingStats(Guid id)
        {
            var res = await groupingService.GetGroupingStats(id);

            var cats = (await categorizationService.GetValidList()).ToList();

            var r1 = cats.Select(x => new GroupingStatsDto()
            {
                category = x.Value,
                count = res.Count(y => y == x.Value).ToString()
            }).ToList();

            var r = cats.ToDictionary(x => x.Value, x => res.Count(y => y == x.Value).ToString());

            return r1;
        }

        public async Task<ExportData> GetGroupingExport(Guid groupId, IEnumerable<Guid> selectedPersons, IEnumerable<Guid> selectedCategorizations, string groupingsType, string rootPath)
        {
            var path = rootPath + @"\Cross Ref";
            var fileName = @"Krydsreference.xlsx";
            var file = new FileInfo(Path.Combine(path, fileName));

            MakeDir(path);
            DeleteFiles(path);

            await CreateExcelFile(file, groupId, selectedPersons, selectedCategorizations, groupingsType);

            return new ExportData()
            {
                RootFolder = path,
                FileName = fileName,
                FileInfo = file
            };
        }

        private async Task CreateExcelFile(FileInfo file, Guid groupId, IEnumerable<Guid> selectedPersons,
            IEnumerable<Guid> selectedCategorizations, string groupingsType)
        {
            using var document = new ExcelPackage(file);

            var personGroupings = await groupingService.GetGroupCrossRef(selectedPersons);

            var group = await groupingService.Get(groupId);

            var personsInGroup = personGroupings.Select(x => x.Person)
                .Distinct()
                .OrderBy(x => x.Name)
                .ToList();
            try
            {
            await PersonXPerson(document, group, personsInGroup, personGroupings, selectedCategorizations, groupingsType);

            PersonXGroup(document, group, personsInGroup, personGroupings, groupingsType);


                document.Save(); //Save the workbook.
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #region PersonXGroup Methods
        private void PersonXGroup(ExcelPackage document, Grouping group, List<Person> personsInGroup,
IEnumerable<PersonGrouping> personGroupings, string groupingsType)
        {
            rows = 2;
            cols = 1;
            var worksheet = document.Workbook.Worksheets.Add("Person X Group");

            maxRows = personsInGroup.Count + 1;

            var groups = personGroupings.Select(x => x.Person).Distinct()
                .SelectMany(x => x.PersonGroupings)
                .Select(x => x.Grouping).Where(x => x.Type == groupingsType)
                .OrderBy(x => x.Value).Distinct()
                .ToList();

            var filteredGroups = groups.DistinctBy(x => x.Value).ToList();

            PersonXGroupHeaders(worksheet, group.Value, filteredGroups);
            PersonsInGroup(worksheet, personsInGroup);

            MarkInGroup(worksheet, personsInGroup, groups);

            FormatWorksheet(worksheet, "PersonXGroup");
        }

        private void MarkInGroup(ExcelWorksheet worksheet, List<Person> personsInGroup, List<Grouping> groupings)
        {
            rows = 2;

            foreach (var person in personsInGroup)
            {
                cols = 7;

                foreach (var grouping in groupings)
                {
                    if (person.PersonGroupings.Any(x => x.Grouping_Id == grouping.Id)) worksheet.Cells[rows, cols].Value = "X";

                    cols++;
                }

                rows++;
            }
        }


        private void PersonXGroupHeaders(ExcelWorksheet worksheet, string groupName, List<Grouping> groups)
        {
            var headerList = new List<string>()
            {
                $"Navn på Gruppering: {groupName}",
                "Cpr.Nr",
                "Sagsområde",
                "Recidiv",
                "Skole",
                "Alder"
            };

            headerList.AddRange(groups.OrderBy(x => x.Value).Select(x => x.Value));

            maxCols = headerList.Count;

            for (var i = 0; i < headerList.Count; i++)
            {
                worksheet.Cells[1, i + 1].Value = headerList[i];
            }
        }
        #endregion

        #region PersonXPerson Methods
        private async Task PersonXPerson(ExcelPackage document, Grouping group, List<Person> personsInGroup,
            IEnumerable<PersonGrouping> personGroupings, IEnumerable<Guid> selectedCategorizations,
            string groupingsType)
        {
            var worksheet = document.Workbook.Worksheets.Add("Person X Person");

            maxRows = personsInGroup.Count + 1;

            var allPersons = (await GetAllPersonsForPersonXPerson(personGroupings, selectedCategorizations.ToList(), groupingsType)).ToList();

            PersonXPersonHeaders(worksheet, group.Value, allPersons);
            PersonsInGroup(worksheet, personsInGroup);

            CalcCrossRef(worksheet, personsInGroup, allPersons);

            FormatWorksheet(worksheet, "PersonXPerson");
        }

        private async Task<IEnumerable<Person>> GetAllPersonsForPersonXPerson(IEnumerable<PersonGrouping> personGroupings, List<Guid> selectedCategorizations, string groupingsType)
        {
            var allPersons = personGroupings
                 .Select(x => x.Person).OrderBy(x => x.Name)
                 .SelectMany(x => x.PersonGroupings)
                 .Select(x => x.Grouping).Distinct()
                 .SelectMany(x => x.PersonGroupings)
                 .Select(x => x.Person).Where(x => x.PersonGroupings.Any(y => y.Grouping.Type == groupingsType))
                 .OrderBy(x => x.Name).Distinct()
                 .ToList();

            var filteredPersons = new List<Person>();

            foreach (var person in allPersons)
            {
                if (selectedCategorizations.Contains((await personService.GetLatestCategorization(person.Id)).Id))
                {
                    filteredPersons.Add(person);
                }
            }

            return filteredPersons;
        }

        private void CalcCrossRef(ExcelWorksheet worksheet, List<Person> personsInGroup, IEnumerable<Person> allPersons)
        {
            rows = 2;

            foreach (var inGroupPerson in personsInGroup)
            {
                cols = 7;

                var memberGroups = inGroupPerson.PersonGroupings.Select(x => x.Grouping_Id);

                foreach (var allPerson in allPersons)
                {
                    if (inGroupPerson == allPerson)
                    {
                        worksheet.Cells[rows, cols].Value = "x";
                        cols++;
                        continue;
                    }

                    var count = allPerson.PersonGroupings.Select(x => x.Grouping_Id).Count(x => memberGroups.Contains(x));

                    worksheet.Cells[rows, cols].Value = count;

                    cols++;
                }

                rows++;
            }
        }


        private void FormatWorksheet(ExcelWorksheet worksheet, string tableName)
        {
            try
            {
                var range = worksheet.Cells[1, 1, maxRows, maxCols];

                var table = worksheet.Tables.Add(range, tableName);
                table.TableStyle = TableStyles.Medium4;

                worksheet.Cells[2, 7, maxRows, maxCols].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                worksheet.Cells.AutoFitColumns();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void PersonsInGroup(ExcelWorksheet worksheet, IEnumerable<Person> personsInGroup)
        {
            rows = 2;
            cols = 1;

            foreach (var person in personsInGroup.OrderBy(x => x.Name))
            {
                worksheet.Cells[rows, cols].Value = person.Name;
                cols++;
                worksheet.Cells[rows, cols].Value = person.SocialSecNum;
                cols++;
                worksheet.Cells[rows, cols].Value = person?.SspArea?.Value;
                cols++;
                worksheet.Cells[rows, cols].Value = person.Worries.Count;
                cols++;
                worksheet.Cells[rows, cols].Value = person.SchoolData?.Name;
                cols++;
                worksheet.Cells[rows, cols].Value = CalcAge(person.Birthday);
                cols = 1;
                rows++;
            }
        }

        private void PersonXPersonHeaders(ExcelWorksheet worksheet, string groupName, IEnumerable<Person> allPersons)
        {
            var headerList = new List<string>()
            {
                $"Navn på Gruppering: {groupName}",
                "Cpr.Nr",
                "Sagsområde",
                "Recidiv",
                "Skole",
                "Alder"
            };

            headerList.AddRange(allPersons.OrderBy(x => x.Name).Select(x => x.Name));

            maxCols = headerList.Count;

            for (var i = 0; i < headerList.Count; i++)
            {
                worksheet.Cells[1, i + 1].Value = headerList[i];
            }
        }

        #endregion
        private object CalcAge(string personBirthday)
        {
            var day = personBirthday.Substring(6, 2);
            var month = personBirthday.Substring(4, 2);
            var year = personBirthday.Substring(0, 4);

            var bDay = DateTime.Parse($"{year}-{month}-{day}");

            var age = DateTime.Now.Subtract(bDay);

            int years = (int)(age.TotalDays / 365.25);

            return years;
        }

        #region FileMethods
        private async Task<string> GetFileName(Guid groupId)
        {
            return (await groupingService.Get(groupId)).Value;
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
        #endregion
    }
}
