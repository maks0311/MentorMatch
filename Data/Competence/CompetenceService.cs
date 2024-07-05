using Dapper;
using Mentor.Data;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System;
using Mentor;
using Mentor.Pages;
using System.Diagnostics;

namespace Mentor.Data
{
    public class CityService : ICityService
    {

        private readonly SqlConnectionConfiguration _configuration;
        public CityService(SqlConnectionConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int Create(CityModel city)
        {
            var parameters = new DynamicParameters();
            parameters.Add("CITY_ID", city.CITY_ID, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("CITY_NAME", city.CITY_NAME, DbType.String);

            int affectedRows;
            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    affectedRows = conn.Execute("SYS_CITY_CREATE", parameters, commandType: CommandType.StoredProcedure);
                    retVal = parameters.Get<int>("CITY_ID");
                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry("Mentor", ex.Message, EventLogEntryType.Error);
                    throw;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return retVal;

        }

        public async Task<int> CreateAsync(CityModel city)
        {
            var parameters = new DynamicParameters();
            parameters.Add("CITY_ID", city.CITY_ID, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("CITY_NAME", city.CITY_NAME, DbType.String);

            int affectedRows;
            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    affectedRows = await conn.ExecuteAsync("SYS_CITY_CREATE", parameters, commandType: CommandType.StoredProcedure);
                    retVal = parameters.Get<int>("CITY_ID");
                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry("Mentor", ex.Message, EventLogEntryType.Error);
                    throw;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return retVal;
        }

        public CityModel Select(int cityID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("CITY_ID", cityID, DbType.Int32);

            CityModel retVal = new();

            using (var conn = new SqlConnection(_configuration.Value))
            {

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = conn.QueryFirstOrDefault<CityModel>("SYS_CITY_SELECT", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry("Mentor", ex.Message, EventLogEntryType.Error);
                    throw;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return retVal;
        }

        public async Task<CityModel> SelectAsync(int cityID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("CITY_ID", cityID, DbType.Int32);

            CityModel retVal = new();

            using (var conn = new SqlConnection(_configuration.Value))
            {

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = await conn.QueryFirstOrDefaultAsync<CityModel>("SYS_CITY_SELECT", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry("Mentor", ex.Message, EventLogEntryType.Error);
                    throw;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return retVal;
        }

        public IEnumerable<CityModel> SelectAll()
        {
            var parameters = new DynamicParameters();
            IEnumerable<CityModel> cityInfoList;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    cityInfoList = conn.Query<CityModel>("SYS_CITY_SELECT_ALL", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry("Mentor", ex.Message, EventLogEntryType.Error);
                    throw;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return cityInfoList;
        }

        public async Task<IEnumerable<CityModel>> SelectAllAsync()
        {
            var parameters = new DynamicParameters();
            IEnumerable<CityModel> cityInfoList;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    cityInfoList = await conn.QueryAsync<CityModel>("SYS_CITY_SELECT_ALL", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry("Mentor", ex.Message, EventLogEntryType.Error);
                    throw;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return cityInfoList;
        }

        public int Update(CityModel city)
        {
            int retVal = 0;

            var parameters = new DynamicParameters();
            parameters.Add("CITY_ID", city.CITY_ID, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("CITY_NAME", city.CITY_NAME, DbType.String);


            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = conn.Execute("SYS_CITY_UPDATE", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry("Mentor", ex.Message, EventLogEntryType.Error);
                    throw;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return retVal;
        }

        public async Task<int> UpdateAsync(CityModel city)
        {
            int retVal = 0;

            var parameters = new DynamicParameters();
            parameters.Add("CITY_ID", city.CITY_ID, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("CITY_NAME", city.CITY_NAME, DbType.String);

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = await conn.ExecuteAsync("SYS_CITY_UPDATE", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry("Mentor", ex.Message, EventLogEntryType.Error);
                    throw;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return retVal;
        }

        public int Delete(int cityID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("CITY_ID", cityID, DbType.Int32);

            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = conn.Execute("SYS_CITY_DELETE", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry("Mentor", ex.Message, EventLogEntryType.Error);
                    throw;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return retVal;
        }

        public async Task<int> DeleteAsync(int cityID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("CITY_ID", cityID, DbType.Int32);

            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = await conn.ExecuteAsync("SYS_CITY_DELETE", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry("Mentor", ex.Message, EventLogEntryType.Error);
                    throw;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return retVal;
        }
    }
}