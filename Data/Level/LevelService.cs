using Dapper;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Reflection;
using Mentor.Pages;

namespace Mentor.Data
{
    public class LevelService : ILevelService
    {
        private static NLog.ILogger AppLogger = NLog.LogManager.GetCurrentClassLogger();

        private readonly SqlConnectionConfiguration _configuration;
        public LevelService(SqlConnectionConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int Create(LevelModel level)
        {
            var parameters = new DynamicParameters();
            parameters.Add("LEVEL_ID", level.LEVEL_ID, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("LEVEL_NAME", level.LEVEL_NAME, DbType.String);
            parameters.Add("IS_ACTIVE", level.IS_ACTIVE, DbType.Boolean);

            int affectedRows;
            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    affectedRows = conn.Execute("SYS_LEVEL_CREATE", parameters, commandType: CommandType.StoredProcedure);
                    retVal = parameters.Get<int>("LEVEL_ID");
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

        public async Task<int> CreateAsync(LevelModel level)
        {
            var parameters = new DynamicParameters();
            parameters.Add("LEVEL_ID", level.LEVEL_ID, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("LEVEL_NAME", level.LEVEL_NAME, DbType.String);
            parameters.Add("IS_ACTIVE", level.IS_ACTIVE, DbType.Boolean);

            int affectedRows;
            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    affectedRows = await conn.ExecuteAsync("SYS_LEVEL_CREATE", parameters, commandType: CommandType.StoredProcedure);
                    retVal = parameters.Get<int>("LEVEL_ID");
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

        public async Task<LevelModel> SelectAsync(int levelID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("LEVEL_ID", levelID, DbType.Int32);

            LevelModel level;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    level = await conn.QueryFirstOrDefaultAsync<LevelModel>("SYS_LEVEL_SELECT", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
                    throw;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return level;
        }

        public LevelModel Select(int levelID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("LEVEL_ID", levelID, DbType.Int32);

            LevelModel level;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    level = conn.QueryFirstOrDefault<LevelModel>("SYS_LEVEL_SELECT", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
                    throw;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return level;
        }

        public async Task<IEnumerable<LevelModel>> SelectAllAsync()
        {
            var parameters = new DynamicParameters();

            IEnumerable<LevelModel> LevelEnum;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    LevelEnum = await conn.QueryAsync<LevelModel>("SYS_LEVEL_SELECT_ALL", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
                    throw;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return LevelEnum;
        }

        public IEnumerable<LevelModel> SelectAll()
        {
            var parameters = new DynamicParameters();

            IEnumerable<LevelModel> LevelEnum;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    LevelEnum = conn.Query<LevelModel>("SYS_LEVEL_SELECT_ALL", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
                    throw;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return LevelEnum;
        }

        public async Task<IEnumerable<LevelModel>> SelectAllByTutorAsync(int tutorID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("TUTOR_ID", tutorID, DbType.Int32);

            IEnumerable<LevelModel> LevelEnum;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    LevelEnum = await conn.QueryAsync<LevelModel>("SYS_LEVEL_SELECT_ALL_BY_TUTOR", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
                    throw;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return LevelEnum;
        }

        public IEnumerable<LevelModel> SelectAllByTutor(int tutorID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("TUTOR_ID", tutorID, DbType.Int32);

            IEnumerable<LevelModel> LevelEnum;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    LevelEnum = conn.Query<LevelModel>("SYS_LEVEL_SELECT_ALL_BY_TUTOR", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
                    throw;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return LevelEnum;
        }

        public int Update(LevelModel level)
        {
            int retVal = 0;

            var parameters = new DynamicParameters();
            parameters.Add("LEVEL_ID", level.LEVEL_ID, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("LEVEL_NAME", level.LEVEL_NAME, DbType.String);
            parameters.Add("IS_ACTIVE", level.IS_ACTIVE, DbType.Boolean);

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = conn.Execute("SYS_LEVEL_UPDATE", parameters, commandType: CommandType.StoredProcedure);
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

        public async Task<int> UpdateAsync(LevelModel level)
        {
            int retVal = 0;

            var parameters = new DynamicParameters();
            parameters.Add("LEVEL_ID", level.LEVEL_ID, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("LEVEL_NAME", level.LEVEL_NAME, DbType.String);
            parameters.Add("IS_ACTIVE", level.IS_ACTIVE, DbType.Boolean);

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = await conn.ExecuteAsync("SYS_LEVEL_UPDATE", parameters, commandType: CommandType.StoredProcedure);
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

        public int Delete(int levelID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("LEVEL_ID", levelID, DbType.Int32);

            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = conn.Execute("SYS_LEVEL_DELETE", parameters, commandType: CommandType.StoredProcedure);
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

        public async Task<int> DeleteAsync(int lessonID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("LEVEL_ID", lessonID, DbType.Int32);

            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = await conn.ExecuteAsync("SYS_LEVEL_DELETE", parameters, commandType: CommandType.StoredProcedure);
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
    }
}
