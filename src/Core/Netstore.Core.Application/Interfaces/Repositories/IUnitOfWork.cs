using Netstore.Core.Application.Interfaces.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Netstore.Core.Application.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable, ITransientService
{
    Task<int> Commit(CancellationToken cancellationToken);
    Task Rollback();
}