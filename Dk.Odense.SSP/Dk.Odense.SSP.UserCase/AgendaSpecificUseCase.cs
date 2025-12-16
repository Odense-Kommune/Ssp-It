using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.UserCase.ViewModel;

namespace Dk.Odense.SSP.UserCase
{
    public class AgendaSpecificUseCase
    {
        private readonly IAgendaItemService agendaItemService;
        private readonly PersonUseCase personUseCase;

        public AgendaSpecificUseCase(IAgendaItemService agendaItemService, PersonUseCase personUseCase)
        {
            this.agendaItemService = agendaItemService;
            this.personUseCase = personUseCase;
        }


        public async Task<IEnumerable<AgendaSpecificMenuItem>> GetMenuItems(Guid agendaId)
        {
            try
            {
                var agendaItems = await agendaItemService.GetExistingItemsOnAgenda(agendaId);

                var retList = agendaItems.Select(x => new AgendaSpecificMenuItem()
                {
                    Id = x.Id,
                    Person = x.Worries.FirstOrDefault()?.Person == null ? new PersonDto() : personUseCase.MapPerson(x.Worries.FirstOrDefault(y => y.AgendaItem.Agenda_Id == agendaId)?.Person, x.Agenda_Id),
                    WorryItems = x.Worries.Select(y => new WorryItem()
                    {
                        Id = y.Id,
                        Increment = y.Increment,
                        Workplace = y.Reporter.Workplace,
                        CrimeScene = y.CrimeScene,
                        PoliceCat = y.PoliceWorryCategory_Id,
                        PoliceRole = y.PoliceWorryRole_Id
                    }),
                    SortOrder = x.SortOrder
                }).ToList();

                return retList.Where(x => x.Person.Id != Guid.Empty);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
