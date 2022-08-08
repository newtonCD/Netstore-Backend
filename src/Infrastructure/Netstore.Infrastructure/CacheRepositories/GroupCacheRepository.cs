using Ardalis.GuardClauses;
using Microsoft.Extensions.Caching.Distributed;
using Netstore.Core.Application.Interfaces.CacheRepositories;
using Netstore.Core.Application.Interfaces.Repositories;
using Netstore.Core.Domain.Entities.Customers;
using Netstore.Infrastructure.CacheKeys;
using Netstore.Infrastructure.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Netstore.Infrastructure.CacheRepositories;

public class GroupCacheRepository : IGroupCacheRepository
{
    private readonly IDistributedCache _distributedCache;
    private readonly IGroupRepository _groupRepository;

    public GroupCacheRepository(IDistributedCache distributedCache, IGroupRepository groupRepository)
    {
        _distributedCache = distributedCache;
        _groupRepository = groupRepository;
    }

    public async Task<Group> GetByIdAsync(int groupId)
    {
        string cacheKey = GroupCacheKeys.GetKey(groupId);
        Group group = await _distributedCache.GetAsync<Group>(cacheKey);
        if (group == null)
        {
            group = await _groupRepository.GetByIdAsync(groupId);
            Guard.Against.Null(group, nameof(group), "Nenhum grupo localizado.");
            await _distributedCache.SetAsync(cacheKey, group);
        }

        return group;
    }

    public async Task<List<Group>> GetCachedListAsync()
    {
        string cacheKey = GroupCacheKeys.ListKey;
        List<Group> groupList = await _distributedCache.GetAsync<List<Group>>(cacheKey);
        if (groupList == null)
        {
            groupList = await _groupRepository.GetListAsync();
            await _distributedCache.SetAsync(cacheKey, groupList);
        }

        return groupList;
    }
}