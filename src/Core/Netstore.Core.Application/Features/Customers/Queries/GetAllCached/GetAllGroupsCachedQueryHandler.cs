using AutoMapper;
using MediatR;
using Netstore.Common.Results;
using Netstore.Core.Application.Interfaces.CacheRepositories;
using Netstore.Core.Domain.Entities.Customers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Netstore.Core.Application.Features.Customers.Queries.GetAllCached;

public class GetAllGroupsCachedQueryHandler : IRequestHandler<GetAllGroupsCachedQuery, Result<List<GroupResponse>>>
{
    private readonly IGroupCacheRepository _groupCache;
    private readonly IMapper _mapper;

    public GetAllGroupsCachedQueryHandler(IGroupCacheRepository groupCache, IMapper mapper)
    {
        _groupCache = groupCache;
        _mapper = mapper;
    }

    public async Task<Result<List<GroupResponse>>> Handle(GetAllGroupsCachedQuery request, CancellationToken cancellationToken)
    {
        List<Group> groupList = await _groupCache.GetCachedListAsync();
        List<GroupResponse> mappedGroups = _mapper.Map<List<GroupResponse>>(groupList);
        return Result<List<GroupResponse>>.Success(mappedGroups);
    }
}