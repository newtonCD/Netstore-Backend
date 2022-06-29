#nullable enable

using Microsoft.AspNetCore.Mvc;

namespace Template.WebApi.Controllers;

[Route("/error")]
public class ErrorsController : ControllerBase
{
    public IActionResult Error()
    {
        return Problem();
    }
}