using Netstore.Core.Application.Interfaces.Services;
using Netstore.Core.Domain.Entities.Customers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Netstore.Core.Application.Interfaces.CacheRepositories;

public interface IGroupCacheRepository : ITransientService
{
    Task<List<Group>> GetCachedListAsync();
    Task<Group> GetByIdAsync(int groupId);
}