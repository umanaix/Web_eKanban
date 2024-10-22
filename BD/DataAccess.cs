using Dapper;
using Dapper.Mapper;
using Entity;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD
{
    public class DataAccess : IDataAccess
    {
        private readonly IConfiguration config;

        public DataAccess(IConfiguration _Config)
        {
            config = _Config;


        }


        public SqlConnection DbConnection => new SqlConnection(
            new SqlConnectionStringBuilder(config.GetConnectionString("Conn")).ConnectionString
            );



        public OracleConnection connection => new OracleConnection(
                 new OracleConnectionStringBuilder(config.GetConnectionString("OracleEBS")).ConnectionString
                 );

        public async Task<IEnumerable<T>> OQueryAsync<T>(string sp, object Param = null, int? Timeout = null)
        {
            try
            {
                using (var exec = connection)
                {
                    await exec.OpenAsync();


                    var result = exec.QueryAsync<T>(sql: sp);

                    return await result;

                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<DBEntity> OQueryAsyncP(string sp, object Param = null, int? Timeout = null)
        {
            try
            {
                using (var exec = connection)
                {
                    await exec.OpenAsync();
                    using (OracleCommand cmd = exec.CreateCommand())
                    {
                        cmd.CommandText = sp;
                        cmd.CommandType = CommandType.Text;

                        // Agregar los parámetros de entrada/salida al comando

                        OracleParameter pIdParam = new OracleParameter("pid", OracleDbType.Int32);
                        pIdParam.Direction = ParameterDirection.InputOutput;
                        cmd.Parameters.Add(pIdParam);

                        OracleParameter pCodeErrorParam = new OracleParameter("pcodeerror", OracleDbType.Int32);
                        pCodeErrorParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(pCodeErrorParam);

                        OracleParameter pMsgErrorParam = new OracleParameter("pmsgerror", OracleDbType.Varchar2);
                        pMsgErrorParam.Size = 2000;
                        pMsgErrorParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(pMsgErrorParam);

                        // Ejecutar la consulta
                        await cmd.ExecuteNonQueryAsync();

                        // Recuperar los valores de los parámetros de salida
                        Int32 codeError = System.Convert.ToInt32(pCodeErrorParam.Value.ToString());
                        Int32 idValue = System.Convert.ToInt32(pIdParam.Value.ToString());
                        string msgError = (string)pMsgErrorParam.Value.ToString();

                        return new() { 
                            CodeError = codeError,
                            MsgError = msgError,
                            Id = idValue
                        };

                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<DBEntity> ExecuteProcedure(string sp, object param = null)
        {
            var parameters = new DynamicParameters();
            parameters = (DynamicParameters)param;
            try
            {
                using (var exec = connection)
                {
                    await exec.OpenAsync();

                    var result = exec.ExecuteAsync(sp, parameters, commandType: CommandType.StoredProcedure);

                    return new()
                    {
                        CodeError = parameters.Get<int>("pcError"),
                        MsgError = parameters.Get<string>("pmError")
                    };
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<T> OQueryFirstAsync<T>(string sp, object Param = null, int? Timeout = null)
        {
            try
            {
                using (var exec = connection)
                {
                    await exec.OpenAsync();

                    if (Param != null)
                    {
                        // Si Param no es nulo, entonces pasamos los parámetros a la consulta
                        var result = exec.QueryFirstOrDefaultAsync<T>(sql: sp, param: Param);
                        return await result;
                    }
                    else
                    {
                        // Si Param es nulo, ejecutamos la consulta sin parámetros
                        var result = exec.QueryFirstOrDefaultAsync<T>(sql: sp);
                        return await result;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<IEnumerable<T>> QueryAsync<T>(string sp, object Param = null, int? Timeout = null)
        {
            try
            {
                using (var exec = DbConnection)
                {
                    await exec.OpenAsync();

                    var result = exec.QueryAsync<T>(sql: sp, param: Param, commandType: System.Data.CommandType.StoredProcedure
                        , commandTimeout: Timeout);

                    return await result;

                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<IEnumerable<dynamic>> QueryAsync(string sp, object Param = null, int? Timeout = null)
        {
            try
            {
                using (var exec = DbConnection)
                {
                    await exec.OpenAsync();

                    var result = exec.QueryAsync(sql: sp, param: Param, commandType: System.Data.CommandType.StoredProcedure
                        , commandTimeout: Timeout);

                    return await result;

                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<IEnumerable<T>> QueryAsync<T, B>(string sp, string split, object Param = null, int? Timeout = null)
        {
            try
            {
                using (var exec = DbConnection)
                {
                    await exec.OpenAsync();

                    var result = exec.QueryAsync<T, B>(sql: sp, param: Param, commandType: System.Data.CommandType.StoredProcedure
                        , commandTimeout: Timeout, splitOn: split);

                    return await result;

                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<IEnumerable<T>> QueryAsync<T, B, C>(string sp, string split, object Param = null, int? Timeout = null)
        {
            try
            {
                using (var exec = DbConnection)
                {
                    await exec.OpenAsync();

                    var result = exec.QueryAsync<T, B, C>(sql: sp, param: Param, commandType: System.Data.CommandType.StoredProcedure
                        , commandTimeout: Timeout, splitOn: split);

                    return await result;

                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<IEnumerable<T>> QueryAsync<T, B, C, D>(string sp, string split, object Param = null, int? Timeout = null)
        {
            try
            {
                using (var exec = DbConnection)
                {
                    await exec.OpenAsync();

                    var result = exec.QueryAsync<T, B, C, D>(sql: sp, param: Param, commandType: System.Data.CommandType.StoredProcedure
                        , commandTimeout: Timeout, splitOn: split);

                    return await result;

                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<IEnumerable<T>> QueryAsync<T, B, C, D, E>(string sp, string split, object Param = null, int? Timeout = null)
        {
            try
            {
                using (var exec = DbConnection)
                {
                    await exec.OpenAsync();

                    var result = exec.QueryAsync<T, B, C, D, E>(sql: sp, param: Param, commandType: System.Data.CommandType.StoredProcedure
                        , commandTimeout: Timeout);

                    return await result;

                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<IEnumerable<T>> QueryAsync<T, B, C, D, E, F>(string sp, string split, object Param = null, int? Timeout = null)
        {
            try
            {
                using (var exec = DbConnection)
                {
                    await exec.OpenAsync();

                    var result = exec.QueryAsync<T, B, C, D, E, F>(sql: sp, param: Param, commandType: System.Data.CommandType.StoredProcedure
                        , commandTimeout: Timeout, splitOn: split);

                    return await result;

                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<IEnumerable<T>> QueryAsync<T, B, C, D, E, F, G>(string sp, string split, object Param = null, int? Timeout = null)
        {
            try
            {
                using (var exec = DbConnection)
                {
                    await exec.OpenAsync();

                    var result = exec.QueryAsync<T, B, C, D, E, F, G>(sql: sp, param: Param, commandType: System.Data.CommandType.StoredProcedure
                        , commandTimeout: Timeout, splitOn: split);

                    return await result;

                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<T> QueryFirstAsync<T>(string sp, object Param = null, int? Timeout = null)
        {
            try
            {
                using (var exec = DbConnection)
                {
                    await exec.OpenAsync();

                    var result = exec.QueryFirstOrDefaultAsync<T>(sql: sp, param: Param, commandType: System.Data.CommandType.StoredProcedure
                        , commandTimeout: Timeout);

                    return await result;

                }

            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<DBEntity> ExecuteAsync(string sp, object Param = null, int? Timeout = null)
        {
            try
            {
                using (var exec = DbConnection)
                {
                    await exec.OpenAsync();

                    var result = await exec.ExecuteReaderAsync(sql: sp, param: Param, commandType:
                        System.Data.CommandType.StoredProcedure
                        , commandTimeout: Timeout);

                    await result.ReadAsync();

                    return new()
                    {
                        CodeError = result.GetInt32(0),
                        MsgError = result.GetString(1)

                    };

                }

            }
            catch (Exception)
            {

                throw;
            }

        }
        //********
        public async Task InsertAsync(string sp, object data)
        {
            using (var exec = connection)
            {
                //var insertQuery = $"INSERT INTO {tableName} (...) VALUES (...)"; // Construye la consulta INSERT aquí
            
                await exec.ExecuteAsync(sp, data);
            }
        }

        public async Task UpdateAsync(string sp, object data, int? id)
        {
            using (var exec = connection)
            {
                //var updateQuery = $"UPDATE {tableName} SET ... WHERE Id = @Id"; // Construye la consulta UPDATE aquí

                //await exec.ExecuteAsync(sp, new { Id = id, ... });
            }
        }
    }
}
