using Netstore.Core.Application.DTOs.Logs;
using Netstore.Core.Application.Interfaces.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Netstore.Core.Application.Interfaces.Repositories;

public interface ILogRepository : ITransientService
{
    Task<List<AuditLogResponse>> GetAuditLogsAsync(string userId);
    Task AddLogAsync(string action, string userId);
}