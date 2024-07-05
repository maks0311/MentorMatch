using Dapper;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;


namespace Mentor.Data
{
    public class WorkTypeService : IWorkTypeService
    {
        private readonly SqlConnectionConfiguration _configuration;
        public WorkTypeService(SqlConnectionConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<WorkTypeModel>> SelectAllAsync()
        {
            var parameters = new DynamicParameters();

            IEnumerable<WorkTypeModel> userWorkTypeEnum;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    userWorkTypeEnum = await conn.QueryAsync<WorkTypeModel>("SYS_WORK_TYPE_SELECT_ALL", parameters, commandType: CommandType.StoredProcedure);
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
            return userWorkTypeEnum;
        }

        public IEnumerable<WorkTypeModel> SelectAll()
        {
            var parameters = new DynamicParameters();

            IEnumerable<WorkTypeModel> userWorkTypeEnum;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    userWorkTypeEnum = conn.Query<WorkTypeModel>("SYS_WORK_TYPE_SELECT_ALL", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry("Mentor", ex.Message, EventLogEntryType.Error); throw;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return userWorkTypeEnum;
        }

    }
}
