using Netstore.Core.Domain.Entities.Base;

namespace Netstore.Core.Domain.Entities.Misc;
public class ConfigurationItem : Entity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
    public bool Enabled { get; set; }
}