using System.ComponentModel.DataAnnotations;

namespace Netstore.Core.Application.Models;

public class Login
{
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; }
}
