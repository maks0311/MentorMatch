using Dapper;
using Mentor.Pages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Mentor.Data
{
    public class AvailabilityService : IAvailabilityService
    {
        private static NLog.ILogger AppLogger = NLog.LogManager.GetCurrentClassLogger();

        private readonly SqlConnectionConfiguration _configuration;
        public AvailabilityService(SqlConnectionConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int Create(int tutorID, DateTime start, DateTime stop)
        {
            var parameters = new DynamicParameters();
            parameters.Add("AVAILABILITY_ID", 0, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("TUTOR_ID", tutorID, DbType.Int32);
            parameters.Add("DATE_START", start, DbType.DateTime);
            parameters.Add("DATE_STOP", stop, DbType.DateTime);

            int affectedRows;
            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    affectedRows = conn.Execute("SYS_AVAILABILITY_CREATE", parameters, commandType: CommandType.StoredProcedure);
                    retVal = parameters.Get<int>("AVAILABILITY_ID");
                }
                catch (Exception ex)
                {
                    AppLogger.Error(ex.Message);
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

        public async Task<int> CreateAsync(int tutorID, DateTime start, DateTime stop)
        {
            var parameters = new DynamicParameters();
            parameters.Add("AVAILABILITY_ID", 0, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("TUTOR_ID", tutorID, DbType.Int32);
            parameters.Add("DATE_START", start, DbType.DateTime);
            parameters.Add("DATE_STOP", stop, DbType.DateTime);

            int affectedRows;
            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    affectedRows = await conn.ExecuteAsync("SYS_AVAILABILITY_CREATE", parameters, commandType: CommandType.StoredProcedure);
                    retVal = parameters.Get<int>("AVAILABILITY_ID");
                }
                catch (Exception ex)
                {
                    AppLogger.Error(ex.Message);
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

        public int Delete(int tutorID, DateTime start, DateTime stop)
        {
            var parameters = new DynamicParameters();
            parameters.Add("TUTOR_ID", tutorID, DbType.Int32);
            parameters.Add("DATE_START", start, DbType.DateTime);
            parameters.Add("DATE_STOP", stop, DbType.DateTime);

            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = conn.Execute("SYS_AVAILABILITY_DELETE", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    AppLogger.Error(ex.Message);
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


        public async Task<int> DeleteAsync(int tutorID, DateTime start, DateTime stop)
        {
            var parameters = new DynamicParameters();
            parameters.Add("TUTOR_ID", tutorID, DbType.Int32);
            parameters.Add("DATE_START", start, DbType.DateTime);
            parameters.Add("DATE_STOP", stop, DbType.DateTime);

            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = await conn.ExecuteAsync("SYS_AVAILABILITY_DELETE", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    AppLogger.Error(ex.Message);
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


        public IEnumerable<AvailabilityModel> SelectAll(int tutorID, DateTime start, DateTime stop)
        {
            var parameters = new DynamicParameters();
            parameters.Add("TUTOR_ID", tutorID, DbType.Int32);
            parameters.Add("DATE_START", start, DbType.DateTime);
            parameters.Add("DATE_STOP", stop, DbType.DateTime);

            IEnumerable<AvailabilityModel> availabilityInfoList;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    availabilityInfoList = conn.Query<AvailabilityModel>("SYS_AVAILABILITY_SELECT_ALL", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    AppLogger.Error(ex.Message);
                    throw;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return availabilityInfoList;
        }


        public async Task<IEnumerable<AvailabilityModel>> SelectAllAsync(int tutorID, DateTime start, DateTime stop)
        {
            var parameters = new DynamicParameters();
            parameters.Add("TUTOR_ID", tutorID, DbType.Int32);
            parameters.Add("DATE_START", start, DbType.DateTime);
            parameters.Add("DATE_STOP", stop, DbType.DateTime);

            IEnumerable<AvailabilityModel> availabilityInfoList;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    availabilityInfoList = await conn.QueryAsync<AvailabilityModel>("SYS_AVAILABILITY_SELECT_ALL", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    AppLogger.Error(ex.Message);
                    throw;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return availabilityInfoList;
        }
    }
}
