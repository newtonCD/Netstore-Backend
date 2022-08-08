using AutoMapper;
using MediatR;
using Netstore.Common.Results;
using Netstore.Core.Application.Interfaces.CacheRepositories;
using Netstore.Core.Domain.Entities.Customers;
using System.Threading;
using System.Threading.Tasks;

namespace Netstore.Core.Application.Features.Customers.Queries.GetById;

public class GetGroupByIdQuery : IRequest<Result<GroupResponse>>
{
    public int Id { get; set; }

    public class GetGroupByIdQueryHandler : IRequestHandler<GetGroupByIdQuery, Result<GroupResponse>>
    {
        private readonly IGroupCacheRepository _groupCache;
        private readonly IMapper _mapper;

        public GetGroupByIdQueryHandler(IGroupCacheRepository groupCache, IMapper mapper)
        {
            _groupCache = groupCache;
            _mapper = mapper;
        }

        public async Task<Result<GroupResponse>> Handle(GetGroupByIdQuery request, CancellationToken cancellationToken)
        {
            Group group = await _groupCache.GetByIdAsync(request.Id);
            GroupResponse mappedGroup = _mapper.Map<GroupResponse>(group);
            return Result<GroupResponse>.Success(mappedGroup);
        }
    }
}