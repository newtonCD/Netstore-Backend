using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Template.Application.Interfaces;

namespace Template.Infrastructure.Persistence;

[ExcludeFromCodeCoverage]
public class SqlDataAccess : ISqlDataAccess
{
    private const string ConnectionString = "DatabaseSettings:DefaultConnection";
    private readonly IConfiguration _config;

    public SqlDataAccess(IConfiguration config)
    {
        _config = config;
    }

    public async Task<IEnumerable<T>> GetAsync<T, TU>(string storedProcedure, TU parameters)
    {
        using IDbConnection connection = new SqlConnection(_config.GetSection(ConnectionString).Get<string>());
        return await connection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task InsertAsync<T>(string storedProcedure, T parameters)
    {
        using IDbConnection connection = new SqlConnection(_config.GetSection(ConnectionString).Get<string>());
        await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
    }

    public Task DeleteAsync<T>(string storedProcedure, T parameters)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync<T>(string storedProcedure, T parameters)
    {
        throw new NotImplementedException();
    }
}