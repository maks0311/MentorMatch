using Dapper;
using System.Data.SqlClient;
using System.Data;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mentor.Data
{
    public class UserService : IUserService
    {

        private readonly SqlConnectionConfiguration _configuration;
        public UserService(SqlConnectionConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int Create(UserModel user)
        {
            var parameters = new DynamicParameters();
            parameters.Add("USER_ID", user.USER_ID, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("USER_FULLNAME", user.USER_FULLNAME, DbType.String);
            parameters.Add("USER_NICKNAME", user.USER_NICKNAME, DbType.String);
            parameters.Add("USER_EMAIL", user.USER_EMAIL, DbType.String);
            parameters.Add("USER_PHONE", user.USER_PHONE, DbType.String);
            parameters.Add("USER_PASS", user.USER_PASS, DbType.String);
            parameters.Add("IS_ACTIVE", user.IS_ACTIVE, DbType.Boolean);
            parameters.Add("USER_DESCRIPTION", user.USER_DESCRIPTION, DbType.String);
            parameters.Add("GROUP_ID", user.GROUP_ID, DbType.Int32);
            parameters.Add("WORK_TYPE_ID", user.WORK_TYPE_ID, DbType.Int32);

            int affectedRows;
            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    affectedRows = conn.Execute("SYS_USER_CREATE", parameters, commandType: CommandType.StoredProcedure);
                    retVal = parameters.Get<int>("USER_ID");
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
        public async Task<int> CreateAsync(UserModel user)
        {
            var parameters = new DynamicParameters();
            parameters.Add("USER_ID", user.USER_ID, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("USER_FULLNAME", user.USER_FULLNAME, DbType.String);
            parameters.Add("USER_NICKNAME", user.USER_NICKNAME, DbType.String);
            parameters.Add("USER_EMAIL", user.USER_EMAIL, DbType.String);
            parameters.Add("USER_PHONE", user.USER_PHONE, DbType.String);
            parameters.Add("USER_PASS", user.USER_PASS, DbType.String);
            parameters.Add("IS_ACTIVE", user.IS_ACTIVE, DbType.Boolean);
            parameters.Add("USER_DESCRIPTION", user.USER_DESCRIPTION, DbType.String);
            parameters.Add("GROUP_ID", user.GROUP_ID, DbType.Int32);
            parameters.Add("WORK_TYPE_ID", user.WORK_TYPE_ID, DbType.Int32);

            int affectedRows;
            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    affectedRows = await conn.ExecuteAsync("SYS_USER_CREATE", parameters, commandType: CommandType.StoredProcedure);
                    retVal = parameters.Get<int>("USER_ID");
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
        public int Delete(int userID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("USER_ID", userID, DbType.Int32);

            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = conn.Execute("SYS_USER_DELETE", parameters, commandType: CommandType.StoredProcedure);
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

        public async Task<int> DeleteAsync(int userID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("USER_ID", userID, DbType.Int32);

            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = await conn.ExecuteAsync("SYS_USER_DELETE", parameters, commandType: CommandType.StoredProcedure);
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

        public UserModel Select(int userID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("USER_ID", userID, DbType.Int32);

            UserModel retVal = new();

            using (var conn = new SqlConnection(_configuration.Value))
            {

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = conn.QueryFirstOrDefault<UserModel>("SYS_USER_SELECT", parameters, commandType: CommandType.StoredProcedure);
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
        public async Task<UserModel> SelectAsync(int userID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("USER_ID", userID, DbType.Int32);

            UserModel retVal = new();

            using (var conn = new SqlConnection(_configuration.Value))
            {

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = await conn.QueryFirstOrDefaultAsync<UserModel>("SYS_USER_SELECT", parameters, commandType: CommandType.StoredProcedure);
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


        public async Task<int> UpdateAsync(UserModel user)
        {
            int retVal = 0;

            var parameters = new DynamicParameters();
            parameters.Add("USER_ID", user.USER_ID, DbType.Int32);
            parameters.Add("USER_FULLNAME", user.USER_FULLNAME, DbType.String);
            parameters.Add("USER_NICKNAME", user.USER_NICKNAME, DbType.String);
            parameters.Add("USER_EMAIL", user.USER_EMAIL, DbType.String);
            parameters.Add("USER_PHONE", user.USER_PHONE, DbType.String);
            parameters.Add("IS_ACTIVE", user.IS_ACTIVE, DbType.Boolean);
            parameters.Add("USER_DESCRIPTION", user.USER_DESCRIPTION, DbType.String);
            parameters.Add("GROUP_ID", user.GROUP_ID, DbType.Int32);
            parameters.Add("WORK_TYPE_ID", user.WORK_TYPE_ID, DbType.Int32);

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = await conn.ExecuteAsync("SYS_USER_UPDATE", parameters, commandType: CommandType.StoredProcedure);
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

        public int Update(UserModel user)
        {
            int retVal = 0;

            var parameters = new DynamicParameters();
            parameters.Add("USER_ID", user.USER_ID, DbType.Int32);
            parameters.Add("USER_FULLNAME", user.USER_FULLNAME, DbType.String);
            parameters.Add("USER_NICKNAME", user.USER_NICKNAME, DbType.String);
            parameters.Add("USER_EMAIL", user.USER_EMAIL, DbType.String);
            parameters.Add("USER_PHONE", user.USER_PHONE, DbType.String);
            parameters.Add("IS_ACTIVE", user.IS_ACTIVE, DbType.Boolean);
            parameters.Add("USER_DESCRIPTION", user.USER_DESCRIPTION, DbType.String);
            parameters.Add("GROUP_ID", user.GROUP_ID, DbType.Int32);
            parameters.Add("WORK_TYPE_ID", user.WORK_TYPE_ID, DbType.Int32);

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = conn.Execute("SYS_USER_UPDATE", parameters, commandType: CommandType.StoredProcedure);
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

        public async Task<IEnumerable<UserModel>> SelectAllAsync()
        {
            var parameters = new DynamicParameters();

            IEnumerable<UserModel> userInfoList;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    userInfoList = await conn.QueryAsync<UserModel>("SYS_USER_SELECT_ALL", parameters, commandType: CommandType.StoredProcedure);
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
            return userInfoList;
        }

        public IEnumerable<UserModel> SelectAll()
        {
            var parameters = new DynamicParameters();

            IEnumerable<UserModel> userInfoList;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    userInfoList = conn.Query<UserModel>("SYS_USER_SELECT_ALL", parameters, commandType: CommandType.StoredProcedure);
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
            return userInfoList;
        }

        public int Authenticate(string userName, string userPass)
        {
            var parameters = new DynamicParameters();
            parameters.Add("USER_NAME", userName, DbType.String);
            parameters.Add("USER_PASS", userPass, DbType.String);

            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = conn.QueryFirst<Int32>("SYS_USER_AUTHENTICATE", parameters, commandType: CommandType.StoredProcedure);
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


        public int PasswordUpdate(int userID, string pass)
        {
            var parameters = new DynamicParameters();
            parameters.Add("USER_ID", userID, DbType.Int32);
            parameters.Add("USER_PASS", pass, DbType.String);

            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    retVal = conn.Execute("SYS_USER_PASSWORD_UPDATE", parameters, commandType: CommandType.StoredProcedure);
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

        public async Task<int> PasswordUpdateAsync(int userID, string pass)
        {
            var parameters = new DynamicParameters();
            parameters.Add("USER_ID", userID, DbType.Int32);
            parameters.Add("USER_PASS", pass, DbType.String);

            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    retVal = await conn.ExecuteAsync("SYS_USER_PASSWORD_UPDATE", parameters, commandType: CommandType.StoredProcedure);
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

        public async Task<UserModel> SelectByEmailAsync(string userEmail)
        {
            var parameters = new DynamicParameters();
            parameters.Add("USER_EMAIL", userEmail, DbType.String);

            UserModel retVal = new();

            using (var conn = new SqlConnection(_configuration.Value))
            {

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = await conn.QueryFirstOrDefaultAsync<UserModel>("SYS_USER_SELECT_BY_NAME", parameters, commandType: CommandType.StoredProcedure);
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

        public UserModel SelectByEmail(string userEmail)
        {
            var parameters = new DynamicParameters();
            parameters.Add("USER_EMAIL", userEmail, DbType.String);

            UserModel retVal = new();

            using (var conn = new SqlConnection(_configuration.Value))
            {

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = conn.QueryFirstOrDefault<UserModel>("SYS_USER_SELECT_BY_EMAIL", parameters, commandType: CommandType.StoredProcedure);
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


        public async Task<UserModel> SelectByNickAsync(string userNick)
        {
            var parameters = new DynamicParameters();
            parameters.Add("USER_NICK", userNick, DbType.String);

            UserModel retVal = new();

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = await conn.QueryFirstOrDefaultAsync<UserModel>("SYS_USER_SELECT_BY_NICK", parameters, commandType: CommandType.StoredProcedure);
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

        public UserModel SelectByNick(string userNick)
        {
            var parameters = new DynamicParameters();
            parameters.Add("USER_NICK", userNick, DbType.String);

            UserModel retVal = new();

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = conn.QueryFirstOrDefault<UserModel>("SYS_USER_SELECT_BY_NICK", parameters, commandType: CommandType.StoredProcedure);
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

    }
}
