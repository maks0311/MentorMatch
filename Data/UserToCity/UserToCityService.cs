using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
namespace Mentor.Data
{
    public class UserToCityService : IUserToCityService
    {

        private readonly SqlConnectionConfiguration _configuration;
        public UserToCityService(SqlConnectionConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int Create(UserToCityModel userToCity)
        {
            var parameters = new DynamicParameters();
            parameters.Add("ID", userToCity.ID, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("TUTOR_ID", userToCity.TUTOR_ID, DbType.Int32);
            parameters.Add("CITY_ID", userToCity.CITY_ID, DbType.Int32);

            int affectedRows;
            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    affectedRows = conn.Execute("SYS_USER_TO_CITY_CREATE", parameters, commandType: CommandType.StoredProcedure);
                    retVal = parameters.Get<int>("ID");
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

        public async Task<int> CreateAsync(UserToCityModel userToCity)
        {
            var parameters = new DynamicParameters();
            parameters.Add("ID", userToCity.ID, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("TUTOR_ID", userToCity.TUTOR_ID, DbType.Int32);
            parameters.Add("CITY_ID", userToCity.CITY_ID, DbType.Int32);

            int affectedRows;
            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    affectedRows = await conn.ExecuteAsync("SYS_USER_TO_CITY_CREATE", parameters, commandType: CommandType.StoredProcedure);
                    retVal = parameters.Get<int>("ID");
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

        public int Delete(int userToCityID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("ID", userToCityID, DbType.Int32);

            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = conn.Execute("SYS_USER_TO_CITY_DELETE", parameters, commandType: CommandType.StoredProcedure);
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

        public async Task<int> DeleteAsync(int userToCityID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("ID", userToCityID, DbType.Int32);

            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = await conn.ExecuteAsync("SYS_USER_TO_CITY_DELETE", parameters, commandType: CommandType.StoredProcedure);
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

        public IEnumerable<UserToCityModel> SelectAllByTutor(int tutorID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("TUTOR_ID", tutorID, DbType.Int32);

            IEnumerable<UserToCityModel> cityInfoList;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    cityInfoList = conn.Query<UserToCityModel>("SYS_CITY_SELECT_ALL_BY_TUTOR", parameters, commandType: CommandType.StoredProcedure);
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

        public async Task<IEnumerable<UserToCityModel>> SelectAllByTutorAsync(int tutorID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("TUTOR_ID", tutorID, DbType.Int32);

            IEnumerable<UserToCityModel> cityInfoList;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    cityInfoList = await conn.QueryAsync<UserToCityModel>("SYS_CITY_SELECT_ALL_BY_TUTOR", parameters, commandType: CommandType.StoredProcedure);
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
    }
}
