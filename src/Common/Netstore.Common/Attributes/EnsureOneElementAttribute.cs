using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Netstore.Common.Attributes;

[System.AttributeUsage(System.AttributeTargets.All, AllowMultiple = false)]
public class EnsureOneElementAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value is not ICollection collection) return false;
        return collection.Count > 0;
    }
}