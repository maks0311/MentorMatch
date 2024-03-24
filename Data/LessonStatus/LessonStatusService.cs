using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Mentor.Data
{
    public class LessonStatusService : ILessonStatusService
    {
        private static NLog.ILogger AppLogger = NLog.LogManager.GetCurrentClassLogger();

        private readonly SqlConnectionConfiguration _configuration;
        public LessonStatusService(SqlConnectionConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int Create(LessonStatusModel status)
        {
            var parameters = new DynamicParameters();
            parameters.Add("LESSON_STATUS_ID", status.LESSON_STATUS_ID, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("LESSON_STATUS_NAME", status.LESSON_STATUS_NAME, DbType.String);
            parameters.Add("LESSON_STATUS_ICON", status.LESSON_STATUS_ICON, DbType.String);
            parameters.Add("LESSON_STATUS_ICON_COLOR", status.LESSON_STATUS_ICON_COLOR, DbType.String);
            parameters.Add("IS_ACTIVE", status.IS_ACTIVE, DbType.Boolean);

            int affectedRows;
            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    affectedRows = conn.Execute("SYS_LESSON_STATUS_CREATE", parameters, commandType: CommandType.StoredProcedure);
                    retVal = parameters.Get<int>("LESSON_STATUS_ID");
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

        public async Task<int> CreateAsync(LessonStatusModel status)
        {
            var parameters = new DynamicParameters();
            parameters.Add("LESSON_STATUS_ID", status.LESSON_STATUS_ID, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("LESSON_STATUS_NAME", status.LESSON_STATUS_NAME, DbType.String);
            parameters.Add("LESSON_STATUS_ICON", status.LESSON_STATUS_ICON, DbType.String);
            parameters.Add("LESSON_STATUS_ICON_COLOR", status.LESSON_STATUS_ICON_COLOR, DbType.String);
            parameters.Add("IS_ACTIVE", status.IS_ACTIVE, DbType.Boolean);


            int affectedRows;
            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    affectedRows = await conn.ExecuteAsync("SYS_LESSON_STATUS_CREATE", parameters, commandType: CommandType.StoredProcedure);
                    retVal = parameters.Get<int>("LESSON_STATUS_ID");
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

        public int Delete(int statusID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("LESSON_STATUS_ID", statusID, DbType.Int32);

            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = conn.Execute("SYS_LESSON_STATUS_DELETE", parameters, commandType: CommandType.StoredProcedure);
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

        public async Task<int> DeleteAsync(int statusID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("LESSON_STATUS_ID", statusID, DbType.Int32);

            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = await conn.ExecuteAsync("SYS_LESSON_STATUS_DELETE", parameters, commandType: CommandType.StoredProcedure);
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

        public LessonModel Select(int statusID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("LESSON_STATUS_ID", statusID, DbType.Int32);

            LessonModel retVal = new();

            using (var conn = new SqlConnection(_configuration.Value))
            {

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = conn.QueryFirstOrDefault<LessonModel>("SYS_LESSON_STATUS_SELECT", parameters, commandType: CommandType.StoredProcedure);
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

        public async Task<LessonStatusModel> SelectAsync(int statusID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("LESSON_STATUS_ID", statusID, DbType.Int32);

            LessonStatusModel retVal = new();

            using (var conn = new SqlConnection(_configuration.Value))
            {

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = await conn.QueryFirstOrDefaultAsync<LessonStatusModel>("SYS_LESSON_STATUS_SELECT", parameters, commandType: CommandType.StoredProcedure);
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

        public int Update(LessonStatusModel status)
        {
            int retVal = 0;

            var parameters = new DynamicParameters();
            parameters.Add("LESSON_STATUS_ID", status.LESSON_STATUS_ID, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("LESSON_STATUS_NAME", status.LESSON_STATUS_NAME, DbType.String);
            parameters.Add("LESSON_STATUS_ICON", status.LESSON_STATUS_ICON, DbType.String);
            parameters.Add("LESSON_STATUS_ICON_COLOR", status.LESSON_STATUS_ICON_COLOR, DbType.String);
            parameters.Add("IS_ACTIVE", status.IS_ACTIVE, DbType.Boolean);


            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = conn.Execute("SYS_LESSON_STATUS_UPDATE", parameters, commandType: CommandType.StoredProcedure);
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

        public async Task<int> UpdateAsync(LessonStatusModel status)
        {
            int retVal = 0;

            var parameters = new DynamicParameters();
            parameters.Add("LESSON_STATUS_ID", status.LESSON_STATUS_ID, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("LESSON_STATUS_NAME", status.LESSON_STATUS_NAME, DbType.String);
            parameters.Add("LESSON_STATUS_ICON", status.LESSON_STATUS_ICON, DbType.String);
            parameters.Add("LESSON_STATUS_ICON_COLOR", status.LESSON_STATUS_ICON_COLOR, DbType.String);
            parameters.Add("IS_ACTIVE", status.IS_ACTIVE, DbType.Boolean);


            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = await conn.ExecuteAsync("SYS_LESSON_STATUS_UPDATE", parameters, commandType: CommandType.StoredProcedure);
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

        public IEnumerable<LessonStatusModel> SelectAll()
        {
            var parameters = new DynamicParameters();

            IEnumerable<LessonStatusModel> lst;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    lst = conn.Query<LessonStatusModel>("SYS_LESSON_STATUS_SELECT_ALL", parameters, commandType: CommandType.StoredProcedure);
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
            return lst;

        }

        public async Task<IEnumerable<LessonStatusModel>> SelectAllAsync()
        {
            var parameters = new DynamicParameters();

            IEnumerable<LessonStatusModel> lst;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    lst = await conn.QueryAsync<LessonStatusModel>("SYS_LESSON_STATUS_SELECT_ALL", parameters, commandType: CommandType.StoredProcedure);
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
            return lst;
        }
    }
}
