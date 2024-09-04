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
    public class GroupService : IGroupService
    {
        private readonly SqlConnectionConfiguration _configuration;
        public GroupService(SqlConnectionConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int Create(GroupModel group)
        {
            var parameters = new DynamicParameters();
            parameters.Add("GROUP_ID", group.GROUP_ID, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("GROUP_FULLNAME", group.GROUP_NAME, DbType.String);

            int affectedRows;
            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    affectedRows = conn.Execute("SYS_GROUP_CREATE", parameters, commandType: CommandType.StoredProcedure);
                    retVal = parameters.Get<int>("GROUP_ID");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.GetType().FullName);
                    Console.WriteLine(ex.Message);
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
        public async Task<int> CreateAsync(GroupModel group)
        {
            var parameters = new DynamicParameters();
            parameters.Add("GROUP_ID", group.GROUP_ID, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("GROUP_FULLNAME", group.GROUP_NAME, DbType.String);

            int affectedRows;
            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    affectedRows = await conn.ExecuteAsync("SYS_GROUP_CREATE", parameters, commandType: CommandType.StoredProcedure);
                    retVal = parameters.Get<int>("GROUP_ID");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.GetType().FullName);
                    Console.WriteLine(ex.Message);
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

        public async Task<IEnumerable<GroupModel>> SelectAllAsync()
        {
            var parameters = new DynamicParameters();

            IEnumerable<GroupModel> userGroupEnum;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    userGroupEnum = await conn.QueryAsync<GroupModel>("SYS_GROUP_SELECT_ALL", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.GetType().FullName);
                    Console.WriteLine(ex.Message);
                    throw;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return userGroupEnum;
        }

        public IEnumerable<GroupModel> SelectAll()
        {
            var parameters = new DynamicParameters();

            IEnumerable<GroupModel> userGroupEnum;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    userGroupEnum = conn.Query<GroupModel>("SYS_GROUP_SELECT_ALL", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.GetType().FullName);
                    Console.WriteLine(ex.Message);
                    throw;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return userGroupEnum;
        }

    }
}
