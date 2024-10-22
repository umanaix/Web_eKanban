using Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BD
{
    public interface IDataAccess
    {
        Task<DBEntity> ExecuteAsync(string sp, object Param = null, int? Timeout = null);
        Task<IEnumerable<dynamic>> QueryAsync(string sp, object Param = null, int? Timeout = null);
        Task<IEnumerable<T>> QueryAsync<T, B, C, D, E, F, G>(string sp, string split, object Param = null, int? Timeout = null);
        Task<IEnumerable<T>> QueryAsync<T, B, C, D, E, F>(string sp, string split, object Param = null, int? Timeout = null);
        Task<IEnumerable<T>> QueryAsync<T, B, C, D, E>(string sp, string split, object Param = null, int? Timeout = null);
        Task<IEnumerable<T>> QueryAsync<T, B, C, D>(string sp, string split, object Param = null, int? Timeout = null);
        Task<IEnumerable<T>> QueryAsync<T, B, C>(string sp, string split, object Param = null, int? Timeout = null);
        Task<IEnumerable<T>> QueryAsync<T, B>(string sp, string split, object Param = null, int? Timeout = null);
        Task<IEnumerable<T>> QueryAsync<T>(string sp, object Param = null, int? Timeout = null);
        Task<T> QueryFirstAsync<T>(string sp, object Param = null, int? Timeout = null);
        Task<IEnumerable<T>> OQueryAsync<T>(string sp, object Param = null, int? Timeout = null);
        Task<DBEntity> OQueryAsyncP(string sp, object Param = null, int? Timeout = null);
        Task<DBEntity> ExecuteProcedure(string sp, object parameters = null);
        Task<T> OQueryFirstAsync<T>(string sp, object Param = null, int? Timeout = null);
        //****
        Task InsertAsync(string tableName, object data);
        Task UpdateAsync(string tableName, object data, int? id);
    }
}