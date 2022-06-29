using System.Collections.Generic;
using System.Threading.Tasks;
using Template.Domain.Entities.Base;

namespace Template.Application.Repositories.Base;

public interface IRepository<T>
    where T : Entity
{
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<T> DeleteAsync(T entity);
}
