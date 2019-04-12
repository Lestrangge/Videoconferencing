using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VideoconferencingBackend.Interfaces.Repositories
{
    public interface IRepository<T> : IDisposable where T : class 
    {
        /// <summary/>
        /// <returns>All T entities</returns>
        Task<IEnumerable<T>> All();
        /// <summary>
        /// Searches for entity
        /// </summary>
        /// <param name="id">Id to search by</param>
        /// <returns>Found entity</returns>
        Task<T> Get(int id);
        /// <summary>
        /// Searches for entity
        /// </summary>
        /// <param name="name">String to search by</param>
        /// <returns>Found entity</returns>
        Task<T> Get(string name);
        /// <summary>
        /// Creates entity 
        /// </summary>
        /// <param name="item">Item to create</param>
        /// <returns>created item</returns>
        Task<T> Create(T item);
        /// <summary>
        /// Deletes entity
        /// </summary>
        /// <param name="item">item to delete</param>
        /// <returns></returns>
        Task<T> Delete(T item);
        /// <summary>
        /// Updates entity in a db
        /// </summary> 
        /// <param name="item">Item to update</param>
        /// <returns>Updated entity</returns>
        Task<T> Update(T item);
        /// <summary>
        /// Returns pageSize of T objects in db found by name
        /// </summary>
        /// <param name="name">name to search for</param>
        /// <param name="page">page number</param>
        /// <param name="pageSize">page size</param>
        /// <returns></returns>
        Task<IEnumerable<T>> Find(string name, int? page = null, int? pageSize = null);
    }
}
