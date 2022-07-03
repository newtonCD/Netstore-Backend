using Netstore.Domain.Entities.Base;
using System;

namespace Netstore.Domain.Entities.Customers;

public class Customer : Entity
{
    public int CustomerId { get; set; }
    public Guid CustomerCode { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? CpfCnpj { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Zip { get; set; }
    public string? Country { get; set; }
    public string? Password { get; set; }
    public string? Salt { get; set; }
    public string? Role { get; set; }
    public string? Status { get; set; }
    public int GroupId { get; set; }
    public Group? CustomerGroup { get; set; }
}
