using System;

namespace Netstore.Core.Domain.Entities.Base;

public abstract class AuditableEntity
{
    public DateTime CreatedOn { get; set; }
    public DateTime LastModifiedOn { get; set; }
    public string CreatedBy { get; set; }
    public string LastModifiedBy { get; set; }
}