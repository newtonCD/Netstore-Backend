using Microsoft.AspNetCore.Mvc;
using Netstore.Common.Paging;
using Netstore.Common.Results;
using Netstore.Core.Application.Features.Customers.Queries;
using Netstore.Core.Application.Features.Customers.Queries.GetAllCached;
using Netstore.Core.Application.Features.Customers.Queries.GetAllPaged;
using Netstore.Core.Application.Features.Customers.Queries.GetById;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Netstore.API.Controllers.V1;

public class GroupController : ApiControllerBase<GroupController>
{
    /// <summary>
    /// Gets all cached.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("getallcached")]
    public async Task<IActionResult> GetAllCached()
    {
        Result<List<GroupResponse>> groups = await Mediator.Send(new GetAllGroupsCachedQuery());
        return Ok(groups);
    }

    /// <summary>
    /// Gets all with paging.
    /// </summary>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="pageSize">Size of the page.</param>
    /// <returns></returns>
    [HttpGet]
    [Route("getwithpaging")]
    public async Task<IActionResult> GetAllWithPaging(int pageNumber, int pageSize)
    {
        PaginatedResult<GroupResponse> groups = await Mediator.Send(new GetAllGroupsQuery(pageNumber, pageSize));
        return Ok(groups);
    }

    /// <summary>
    /// Gets the by identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        Result<GroupResponse> group = await Mediator.Send(new GetGroupByIdQuery() { Id = id });
        return Ok(group);
    }
}