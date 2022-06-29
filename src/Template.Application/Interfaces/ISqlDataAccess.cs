using System.Collections.Generic;
using System.Threading.Tasks;

namespace Template.Application.Interfaces;

public interface ISqlDataAccess
{
    Task<IEnumerable<T>> GetAsync<T, TU>(string storedProcedure, TU parameters);
    Task InsertAsync<T>(string storedProcedure, T parameters);
    Task DeleteAsync<T>(string storedProcedure, T parameters);
    Task UpdateAsync<T>(string storedProcedure, T parameters);
}
