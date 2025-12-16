using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.Core;
using Dk.Odense.SSP.Domain.Model;
using Dk.Odense.SSP.UserCase.ViewModel;

namespace Dk.Odense.SSP.UserCase
{
    public class AgendaOverviewUseCase
    {
        private readonly IAgendaService agendaService;
        private readonly IWorryService worryService;
        private readonly IPersonService personService;
        private readonly IUnitOfWork unitOfWork;
        private readonly IAgendaItemService agendaItemService;
        private readonly IReportedPersonService reportedPersonService;
        private readonly CalcWeekNumber calcWeekNumber;
        public AgendaOverviewUseCase(IAgendaService agendaService, IWorryService worryService, IPersonService personService, IUnitOfWork unitOfWork, IAgendaItemService agendaItemService, IReportedPersonService reportedPersonService, CalcWeekNumber calcWeekNumber)
        {
            this.agendaService = agendaService;
            this.worryService = worryService;
            this.personService = personService;
            this.unitOfWork = unitOfWork;
            this.agendaItemService = agendaItemService;
            this.reportedPersonService = reportedPersonService;
            this.calcWeekNumber = calcWeekNumber;
        }

        public async Task<IEnumerable<Agenda>> GetHeldAgendas()
        {
            return await agendaService.GetHeldAgendas();
        }

        /** 
         * In order to not store reported person indefinitely, we delete them when agendas are archived
         * We archive when the condition holds that agenda.MeetingHeld = true
         */
        public async Task<bool> UpdateAgenda(Agenda agenda)
        {
            if (agenda.MeetingHeld)
            {
                var items = await agendaItemService.GetAgendaItemsWithWorriesFromAgendaId(agenda.Id);
                foreach (var item in items)
                {
                    foreach (var worry in item.Worries)
                    {
                        if (worry.ReportedPerson_Id == null) continue;

                        await reportedPersonService.Delete((Guid)worry.ReportedPerson_Id);
                    }
                }
            }
            agendaService.Update(agenda);
            await unitOfWork.Commit();
            return true;
        }

        public async Task<IEnumerable<Agenda>> GetCurrentAgendas()
        {
            var agendas = await agendaService.GetCurrentAgendasWithAgendaItems();

            return agendas;
        }

        public async Task<IEnumerable<PendingWorry>> GetPendingWorries()
        {
            var res = await personService.GetPersonWithPendingWorries();

            return res.Select(x => new PendingWorry()
            {
                Person = x,
                Worries = x.Worries.ToList()
            });
        }

        public async Task<bool> CreateAgendaItems(IEnumerable<PendingWorry> items)
        {
            var pendingWorries = items.ToList();
            var existingItems = await agendaItemService.GetExistingItemsOnAgendaNoIncludes(pendingWorries.ToList()[0].AgendaId);

            var agendaItems = existingItems.ToList();
            var order = agendaItems.ToList().Count;
            items = pendingWorries.OrderBy(o => o.Worries.Min(s => s.Increment)).ToList();
            foreach (var item in items)
            {
                // if item already exists
                var x = agendaItems.SingleOrDefault(q => q.Worries.FirstOrDefault(z => z.Person_Id == item.Person.Id) != null);
                if (x != null)
                {
                    item.Worries.ForEach(worry =>
                    {
                        worry.AgendaItem_Id = x.Id;
                        worryService.Update(worry);
                    });
                }
                else
                {
                    // new item.
                    order++;
                    var agendaItem = await agendaItemService.Create(new AgendaItem());
                    agendaItem.Agenda_Id = item.AgendaId;
                    agendaItem.SortOrder = order;
                    item.Worries.ForEach(worry =>
                    {
                        worry.AgendaItem_Id = agendaItem.Id;
                        worryService.Update(worry);
                    });
                }
            }
            await unitOfWork.Commit();
            return true;
        }

        public Agenda UpdateMeetDate(Agenda agenda)
        {
            updateAgendaNumber(agenda);
            var val = agendaService.Update(agenda);
            unitOfWork.Commit().Wait();
            if (val.AgendaItems != null)
            {
                foreach (var item in val.AgendaItems)
                    item.Agenda = null;
            }

            return val;
        }

        public async Task<Agenda> CreateNewAgenda(Agenda agenda)
        {
            updateAgendaNumber(agenda);
            var result = await agendaService.Create(agenda);
            await unitOfWork.Commit();
            return result;
        }

        private void updateAgendaNumber(Agenda agenda)
        {
            agenda.AgendaNumber = calcWeekNumber.GetWeekNumber(agenda.Date);
        }
    }
}
