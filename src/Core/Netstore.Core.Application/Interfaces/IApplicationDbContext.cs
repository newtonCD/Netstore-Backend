using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Netstore.Core.Domain.Entities.Customers;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Netstore.Core.Application.Interfaces;

public interface IApplicationDbContext
{
    IDbConnection Connection { get; }
    bool HasChanges { get; }
    DbSet<Group> Groups { get; set; }
    EntityEntry Entry(object entity);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
