using Netstore.Core.Application.Interfaces.Services;
using Netstore.Core.Domain.Entities.Customers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Netstore.Core.Application.Interfaces.Repositories;

public interface IGroupRepository : ITransientService
{
    IQueryable<Group> Groups { get; }
    Task<List<Group>> GetListAsync();
    Task<Group> GetByIdAsync(int groupId);
    Task<int> InsertAsync(Group group);
    Task UpdateAsync(Group group);
    Task DeleteAsync(Group group);
}