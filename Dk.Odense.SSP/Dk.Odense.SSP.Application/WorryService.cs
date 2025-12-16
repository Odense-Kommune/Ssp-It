using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.Domain.Interfaces;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Application
{
    public class WorryService(IWorryRepository worryRepository) : BaseService<Worry, IWorryRepository>(worryRepository), IWorryService
    {

        private readonly IWorryRepository worryRepository = worryRepository;

        public async Task<int> GetCountByPersonId(Guid personId)
        {
            return await worryRepository.GetCountByPersonId(personId);
        }

        public async Task SetGroundless(Guid id)
        {
            var entity = await Get(id);

            entity.Groundless = DateTime.Now.Date;
            entity.Approved = false;
            entity.AgendaItem_Id = null;

            Update(entity);
        }

        public Task<IEnumerable<Worry>> GetGroundless()
        {
            var groundless = worryRepository.GetGroundless();

            return groundless;
        }

        public async Task<bool> Unverify(Guid id)
        {
            var entity = await Get(id);

            entity.Approved = false;
            entity.Groundless = null;
            entity.Processed = null;
            entity.AgendaItem_Id = null;

            Update(entity);

            return true;
        }

        public async Task<bool> SetPoliceWorryRole(Guid id, Guid policeWorryRoleId)
        {
            var entity = await Get(id);

            entity.PoliceWorryRole_Id = policeWorryRoleId;

            try
            {
                Update(entity);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public async Task<bool> SetPoliceWorryCategory(Guid id, Guid policeWorryCategoryId)
        {
            var entity = await Get(id);

            entity.PoliceWorryCategory_Id = policeWorryCategoryId;

            try
            {
                Update(entity);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public async Task<Guid> AgendaItemCleanup(Guid id)
        {
            var entityList = await Repository.GetWithAgendaItems(id);

            if (entityList?.AgendaItem_Id == null) return Guid.Empty;
            var entity = entityList.AgendaItem;

            return entity.Worries.Count == 1 ? entity.Id : Guid.Empty;
        }


        public async Task<Worry> GetNext(Guid personId, int increment)
        {
            return await Repository.GetNext(personId, increment);
        }

        public async Task<Worry> GetPrevious(Guid personId, int increment)
        {
            return await Repository.GetPrevious(personId, increment);
        }

        public async Task<IEnumerable<Worry>> GetToBeVerified()
        {
            return await Repository.GetToBeVerified();
        }

        public async Task<IEnumerable<Worry>> GetToBeVerifiedWithIncludes()
        {
            return await Repository.GetToBeVerifiedWithIncludes();
        }

        public async Task<IEnumerable<Worry>> GetFromPersonId(Guid personId)
        {
            return await Repository.GetFromPersonId(personId);
        }

        public async Task<IEnumerable<Worry>> GetNotPendingWithIncludes()
        {
            return await Repository.GetNotPendingWithIncludes();
        }

        public async Task<Worry> GetWithIncludes(Guid id)
        {
            var res= await Repository.GetWithIncludes(id);

            if (res?.Concern?.CrimeConcern != null)
                res.Concern.CrimeConcern = res.Concern.CrimeConcern.Replace(@"\r\n", Environment.NewLine);

            return res;
        }
    }
}
