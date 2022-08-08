using MediatR;
using Netstore.Common.Results;
using System.Collections.Generic;

namespace Netstore.Core.Application.Features.Customers.Queries.GetAllCached;

public class GetAllGroupsCachedQuery : IRequest<Result<List<GroupResponse>>>
{
}