using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Netstore.Core.Application.DTOs.Logs;
using Netstore.Core.Application.Interfaces.Repositories;
using Netstore.Core.Application.Interfaces.Services;
using Netstore.Core.Application.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Netstore.Infrastructure.Repositories;

public class LogRepository : ILogRepository
{
    private readonly IMapper _mapper;
    private readonly IRepositoryAsync<Audit> _repository;
    private readonly IDateTimeService _dateTimeService;

    public LogRepository(IRepositoryAsync<Audit> repository, IMapper mapper, IDateTimeService dateTimeService)
    {
        _repository = repository;
        _mapper = mapper;
        _dateTimeService = dateTimeService;
    }

    public async Task AddLogAsync(string action, string userId)
    {
        var audit = new Audit()
        {
            Type = action,
            UserId = userId,
            DateTime = _dateTimeService.NowUtc
        };
        await _repository.AddAsync(audit);
    }

    public async Task<List<AuditLogResponse>> GetAuditLogsAsync(string userId)
    {
        List<Audit> logs = await _repository.Entities.Where(a => a.UserId == userId).OrderByDescending(a => a.Id).Take(250).ToListAsync();
        List<AuditLogResponse> mappedLogs = _mapper.Map<List<AuditLogResponse>>(logs);
        return mappedLogs;
    }
}