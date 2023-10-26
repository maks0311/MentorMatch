using Dapper;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Reflection;

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
    }
}
