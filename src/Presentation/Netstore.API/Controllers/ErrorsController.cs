#nullable enable

using Microsoft.AspNetCore.Mvc;

namespace Netstore.API.Controllers;

[Route("/error")]
public class ErrorsController : ControllerBase
{
    public IActionResult Error()
    {
        return Problem();
    }
}