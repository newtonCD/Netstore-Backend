using MediatR;
using Netstore.Common.Extensions;
using Netstore.Common.Paging;
using Netstore.Core.Application.Interfaces.Repositories;
using Netstore.Core.Domain.Entities.Customers;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Netstore.Core.Application.Features.Customers.Queries.GetAllPaged;

public class GetAllGroupsQueryHandler : IRequestHandler<GetAllGroupsQuery, PaginatedResult<GroupResponse>>
{
    private readonly IGroupRepository _repository;

    public GetAllGroupsQueryHandler(IGroupRepository repository)
    {
        _repository = repository;
    }

    public async Task<PaginatedResult<GroupResponse>> Handle(GetAllGroupsQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Group, GroupResponse>> expression = e => new GroupResponse
        {
            Id = e.Id,
            Name = e.Name,
            Description = e.Description,
            Discount = e.Discount,
            Enabled = e.Enabled
        };

        return await _repository.Groups
            .Select(expression)
            .ToPaginatedListAsync(request.PageNumber, request.PageSize);
    }
}