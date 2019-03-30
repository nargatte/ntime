using System.Collections.Generic;
using System.Threading.Tasks;
using BaseCore.Models;

namespace BaseCore.DataBase
{
    public interface IRepository<T> where T : class, IEntityId
    {
        Task<T> AddAsync(T item);
        Task AddRangeAsync(IEnumerable<T> items);
        Task<T[]> GetAllAsync();
        Task<PageViewModel<T>> GetAllAsync(PageBindingModel pageBindingModel);
        Task<T> GetByEntity(T e);
        Task<T> GetById(int id);
        Task RemoveAllAsync();
        Task RemoveAsync(T item);
        Task RemoveRangeAsync(IEnumerable<T> items);
        Task UpdateAsync(T item);
        Task UpdateRangeAsync(IEnumerable<T> items);
    }
}