using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace Mentor.Data
{
    public class PermissionService : IPermissionService
    {
        private readonly SqlConnectionConfiguration _configuration;
        public PermissionService(SqlConnectionConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> CreateAsync(PermissionModel permission)
        {
            var parameters = new DynamicParameters();
            parameters.Add("GROUP_ID", permission.USER_GROUP_ID, DbType.Int32);
            parameters.Add("ELEMENT_ID", permission.ELEMENT_ID, DbType.Int32);
            parameters.Add("IS_SELECTED", permission.IS_SELECTED, DbType.Boolean);

            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = await conn.ExecuteAsync("SYS_PERMISSION_CREATE", parameters, commandType: CommandType.StoredProcedure);
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

        public int Create(PermissionModel permission)
        {
            var parameters = new DynamicParameters();
            parameters.Add("GROUP_ID", permission.USER_GROUP_ID, DbType.Int32);
            parameters.Add("ELEMENT_ID", permission.ELEMENT_ID, DbType.Int32);
            parameters.Add("IS_SELECTED", permission.IS_SELECTED, DbType.Boolean);

            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = conn.Execute("SYS_PERMISSION_CREATE", parameters, commandType: CommandType.StoredProcedure);
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

        public async Task<int> UpdateAsync(PermissionModel permission)
        {
            var parameters = new DynamicParameters();
            parameters.Add("GROUP_ID", permission.USER_GROUP_ID, DbType.Int32);
            parameters.Add("ELEMENT_ID", permission.ELEMENT_ID, DbType.Int32);
            parameters.Add("IS_SELECTED", permission.IS_SELECTED, DbType.Boolean);

            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = await conn.ExecuteAsync("SYS_PERMISSION_UPDATE", parameters, commandType: CommandType.StoredProcedure);
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

        public int Update(PermissionModel permission)
        {
            var parameters = new DynamicParameters();
            parameters.Add("GROUP_ID", permission.USER_GROUP_ID, DbType.Int32);
            parameters.Add("ELEMENT_ID", permission.ELEMENT_ID, DbType.Int32);
            parameters.Add("IS_SELECTED", permission.IS_SELECTED, DbType.Boolean);

            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = conn.Execute("SYS_PERMISSION_UPDATE", parameters, commandType: CommandType.StoredProcedure);
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

        public IEnumerable<PermissionModel> Select(int GrouoID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("GROUP_ID", GrouoID, DbType.Int32);

            IEnumerable<PermissionModel> retVal;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    retVal = conn.Query<PermissionModel>("SYS_PERMISSION_SELECT", parameters, commandType: CommandType.StoredProcedure);
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

        public async Task<IEnumerable<PermissionModel>> SelectAsync(int GrouoID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("GROUP_ID", GrouoID, DbType.Int32);

            IEnumerable<PermissionModel> retVal;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    retVal = await conn.QueryAsync<PermissionModel>("SYS_PERMISSION_SELECT", parameters, commandType: CommandType.StoredProcedure);
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
