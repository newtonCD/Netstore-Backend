using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Domain.Entities.Base;
public interface IEntityBase<out TId>
{
    TId Id { get; }
}
