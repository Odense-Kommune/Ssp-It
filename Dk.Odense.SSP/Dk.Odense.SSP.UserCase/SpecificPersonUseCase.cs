using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.UserCase.ViewModel;

namespace Dk.Odense.SSP.UserCase
{
    public class SpecificPersonUseCase
    {
        private IWorryService worryService;

        public SpecificPersonUseCase(IWorryService worryService)
        {
            this.worryService = worryService;
        }

        public async Task<IEnumerable<PersonMenuItem>> GetMenuItems(Guid personId)
        {
            var ret = (await worryService.GetFromPersonId(personId)).Select(x => new PersonMenuItem()
            {
                Id = x.Id,
                Value = x.AgendaItem_Id != null
                    ? x.Reporter.Workplace == null
                        ? $"#{x.Increment}"
                        : $"#{x.Increment} - {x.AgendaItem.Agenda.AgendaName} {x.AgendaItem.Agenda.AgendaNumber}.{x.AgendaItem.Agenda.Date:yy} {x.Reporter.Workplace}"
                    : x.Reporter.Workplace == null
                        ? $"#{x.Increment}"
                        : $"#{x.Increment} {x.Reporter.Workplace}",
                AgendaId = x.AgendaItem_Id ?? Guid.Empty
            }).ToList();

            return ret;
        }
    }
}
