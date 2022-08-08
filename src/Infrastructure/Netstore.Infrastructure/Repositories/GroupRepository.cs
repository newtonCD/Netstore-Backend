using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Netstore.Core.Application.Interfaces.Repositories;
using Netstore.Core.Domain.Entities.Customers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Netstore.Infrastructure.Repositories;

public class GroupRepository : IGroupRepository
{
    private readonly IRepositoryAsync<Group> _repository;
    private readonly IDistributedCache _distributedCache;

    public GroupRepository(IDistributedCache distributedCache, IRepositoryAsync<Group> repository)
    {
        _distributedCache = distributedCache;
        _repository = repository;
    }

    public IQueryable<Group> Groups => _repository.Entities;

    public async Task DeleteAsync(Group group)
    {
        await _repository.DeleteAsync(group);
        await _distributedCache.RemoveAsync(CacheKeys.GroupCacheKeys.ListKey);
        await _distributedCache.RemoveAsync(CacheKeys.GroupCacheKeys.GetKey(group.Id));
    }

    public async Task<Group> GetByIdAsync(int groupId)
    {
        return await _repository.Entities.Where(p => p.Id == groupId).FirstOrDefaultAsync();
    }

    public async Task<List<Group>> GetListAsync()
    {
        return await _repository.Entities.ToListAsync();
    }

    public async Task<int> InsertAsync(Group group)
    {
        await _repository.AddAsync(group);
        await _distributedCache.RemoveAsync(CacheKeys.GroupCacheKeys.ListKey);
        return group.Id;
    }

    public async Task UpdateAsync(Group group)
    {
        await _repository.UpdateAsync(group);
        await _distributedCache.RemoveAsync(CacheKeys.GroupCacheKeys.ListKey);
        await _distributedCache.RemoveAsync(CacheKeys.GroupCacheKeys.GetKey(group.Id));
    }
}