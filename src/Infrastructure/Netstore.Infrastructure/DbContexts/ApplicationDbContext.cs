using Microsoft.EntityFrameworkCore;
using Netstore.Core.Application.Interfaces;
using Netstore.Core.Application.Interfaces.Services;
using Netstore.Core.Domain.Entities.Base;
using Netstore.Core.Domain.Entities.Customers;
using Netstore.Infrastructure.DbContexts.Auditing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Netstore.Infrastructure.DbContexts;

public class ApplicationDbContext : AuditableContext, IApplicationDbContext
{
    private readonly IDateTimeService _dateTime;
    private readonly IAuthenticatedUserService _authenticatedUser;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDateTimeService dateTime, IAuthenticatedUserService authenticatedUser)
        : base(options)
    {
        _dateTime = dateTime;
        _authenticatedUser = authenticatedUser;
    }

    public IDbConnection Connection => Database.GetDbConnection();
    public bool HasChanges => ChangeTracker.HasChanges();
    public DbSet<Group> Groups { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>().ToList())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedOn = _dateTime.NowUtc;
                    entry.Entity.CreatedBy = _authenticatedUser.UserId;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedOn = _dateTime.NowUtc;
                    entry.Entity.LastModifiedBy = _authenticatedUser.UserId;
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var property in modelBuilder.Model.GetEntityTypes()
        .SelectMany(t => t.GetProperties())
        .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
        {
            property.SetColumnType("decimal(18,2)");
        }

        base.OnModelCreating(modelBuilder);
    }
}