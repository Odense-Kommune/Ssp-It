using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dk.Odense.SSP.Web.Controllers.Interfaces
{
    public interface IApiController<TKey, TValue> where TValue : class, new()
    {
        /// <summary>
        /// Returns T represented by the primary key id
        /// </summary>
        /// <param name="id">The id of T</param>
        /// <returns>An instance of T</returns>
        /// <remarks>Will set HTTP Status code 404 if id is not valid.</remarks>
        Task<TValue> Get(TKey id);

        /// <summary>
        /// Returns all instances of T in the system relevant to the context
        /// </summary>
        /// <returns>An enumerable of T</returns>
        /// <remarks>Will set HTTP Status code 405 if this operation is not allowed.</remarks>
        Task<IEnumerable<TValue>> List();

        /// <summary>
        /// Creates the instance T in the system.
        /// </summary>
        /// <param name="value">The instance of T to be created</param>
        /// <remarks>Will set XXX status if the operation could not be completed</remarks>
        Task<TValue> Post(TValue value);

        /// <summary>
        /// Updates the instance T
        /// </summary>
        /// <param name="value">The value of T</param>
		Task<TValue> Put(TValue value);

        /// <summary>
        /// Deletes the instance T represented by instance of T
        /// </summary>
        /// <param name="id">The instance of T</param>
        /// <remarks>Will set status 405 Method not allowed if the object can't be deleted</remarks>
        Task<bool> Delete(Guid id);

        /// <summary>
        /// Returns a blank preinitialized instance of T. 
        /// </summary>
        /// <returns>An instance of T prefilled with relevant information, suitable for posting to the API.</returns>
        /// <remarks>Returns status 405 Not allowed if this object can't be created.</remarks>
        Task<TValue> Create();

    }
}
