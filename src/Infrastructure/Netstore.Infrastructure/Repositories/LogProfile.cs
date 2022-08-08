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

public class LogProfile : Profile
{
    public LogProfile()
    {
        CreateMap<AuditLogResponse, Audit>().ReverseMap();
    }
}