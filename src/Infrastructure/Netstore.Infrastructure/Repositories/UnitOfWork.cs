using Netstore.Core.Application.Interfaces.Repositories;
using Netstore.Core.Application.Interfaces.Services;
using Netstore.Infrastructure.DbContexts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Netstore.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly IAuthenticatedUserService _authenticatedUserService;
    private readonly ApplicationDbContext _dbContext;
    private bool _disposed;

    public UnitOfWork(ApplicationDbContext dbContext, IAuthenticatedUserService authenticatedUserService)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _authenticatedUserService = authenticatedUserService;
    }

    public async Task<int> Commit(CancellationToken cancellationToken)
    {
        // Auditable Commits
        if (_authenticatedUserService.UserId == null)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
        else
        {
            return await _dbContext.SaveChangesAsync(_authenticatedUserService.UserId);
        }
    }

    public Task Rollback()
    {
        // TODO
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            // dispose managed resources
            _dbContext.Dispose();
        }

        // dispose unmanaged resources
        _disposed = true;
    }
}