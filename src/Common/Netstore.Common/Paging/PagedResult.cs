// https://www.codingame.com/playgrounds/5363/paging-with-entity-framework-core
using System.Collections.Generic;

namespace Netstore.Common.Paging;

public class PagedResult<T> : PagedResultBase
    where T : class
{
    public PagedResult()
    {
        Results = new List<T>();
    }

    public IList<T> Results { get; set; }
}