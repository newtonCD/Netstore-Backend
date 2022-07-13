using Microsoft.AspNetCore.Mvc;

namespace Netstore.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
}