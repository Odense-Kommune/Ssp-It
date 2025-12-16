using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.Core;
using Dk.Odense.SSP.Web.Controllers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dk.Odense.SSP.Web.Controllers.Api.NonViewModel
{

    //TODO All Read actions need to have legal logging
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Policy = "SSP")]
    public class BaseController<TValue> : ControllerBase, IApiController<Guid, TValue>
        where TValue : class, IEntity, new()
    {
        private readonly IBaseService<TValue> service;
        private readonly IUnitOfWork unitOfWork;

        public BaseController(IBaseService<TValue> service, IUnitOfWork unitOfWork)
        {
            this.service = service;
            this.unitOfWork = unitOfWork;
        }

        [HttpGet("{id}")]
        public virtual async Task<TValue> Get(Guid id)
        {
            try
            {
                return await service.Get(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public virtual async Task<IEnumerable<TValue>> List()
        {
            try
            {
                return await service.List();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        public virtual async Task<TValue> Post(TValue value)
        {
            try
            {
                var ret = await service.Create(value);
                await unitOfWork.Commit();
                return ret;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        public virtual async Task<TValue> Create()
        {
            try
            {
                var value = new TValue();
                return await service.Create(value);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPut]
        public virtual async Task<TValue> Put(TValue value)
        {
            try
            {
                var ret = service.Update(value);
                await unitOfWork.Commit();
                return ret;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpDelete]
        public virtual async Task<bool> Delete(Guid id)
        {
            try
            {
                var ret = await service.Delete(id);
                await unitOfWork.Commit();
                return ret;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}