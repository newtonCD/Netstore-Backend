using MediatR;
using Netstore.Common.Paging;

namespace Netstore.Core.Application.Features.Customers.Queries.GetAllPaged;

public class GetAllGroupsQuery : IRequest<PaginatedResult<GroupResponse>>
{
    public GetAllGroupsQuery(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }

    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
