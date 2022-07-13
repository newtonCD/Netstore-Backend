using Netstore.Domain.Common.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Netstore.Application.Repositories.Base;

public interface IRepository<T>
    where T : Entity<T>
{
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<T> DeleteAsync(T entity);
}