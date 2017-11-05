using System.Collections.Generic;
using System.Threading.Tasks;

namespace BaseCore.DataBase
{
    public interface IRepository<T>
        where T : class, IEntityId
    {
        Task<int> AddAsync(T item);
        Task AddRangeAsync(IEnumerable<T> items);
        Task UpdateAsync(T item);
        Task RemoveAsync(T item);
    }
}