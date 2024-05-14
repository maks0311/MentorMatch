using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;
using System;

namespace Mentor.Data
{
    public class UserNotificationService : IUserNotificationService
    {
        private static NLog.ILogger AppLogger = NLog.LogManager.GetCurrentClassLogger();

        private readonly SqlConnectionConfiguration _configuration;
        public UserNotificationService(SqlConnectionConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<UserNotificationModel> SelectAllByUser(int userID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("USER_ID", userID, DbType.Int32);

            IEnumerable<UserNotificationModel> NotificationEnum;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    NotificationEnum = conn.Query<UserNotificationModel>("SYS_USER_NOTIFICATION_SELECT_ALL_BY_USER", parameters, commandType: CommandType.StoredProcedure);
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
            return NotificationEnum;
        }

        public async Task<IEnumerable<UserNotificationModel>> SelectAllByUserAsync(int userID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("USER_ID", userID, DbType.Int32);

            IEnumerable<UserNotificationModel> NotificationEnum;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    NotificationEnum = await conn.QueryAsync<UserNotificationModel>("SYS_USER_NOTIFICATION_SELECT_ALL_BY_USER", parameters, commandType: CommandType.StoredProcedure);
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
            return NotificationEnum;
        }

        public int Create(UserNotificationModel userNotification)
        {
            var parameters = new DynamicParameters();
            parameters.Add("ID", 0, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("NOTIFICATION_ID", userNotification.NOTIFICATION_ID, DbType.Int32);
            parameters.Add("LESSON_ID", userNotification.LESSON_ID, DbType.Int32);
            parameters.Add("TUTOR_ID", userNotification.TUTOR_ID, DbType.Int32);
            parameters.Add("STUDENT_ID", userNotification.STUDENT_ID, DbType.Int32);

            int affectedRows;
            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    affectedRows = conn.Execute("SYS_USER_NOTIFICATION_CREATE", parameters, commandType: CommandType.StoredProcedure);
                    retVal = parameters.Get<int>("NOTIFICATION_ID");
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

        public async Task<int> CreateAsync(UserNotificationModel userNotification)
        {
            var parameters = new DynamicParameters();
            parameters.Add("ID", userNotification.ID, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("NOTIFICATION_ID", userNotification.NOTIFICATION_ID, DbType.Int32);
            parameters.Add("LESSON_ID", userNotification.LESSON_ID, DbType.Int32);
            parameters.Add("TUTOR_ID", userNotification.TUTOR_ID, DbType.Int32);
            parameters.Add("STUDENT_ID", userNotification.STUDENT_ID, DbType.Int32);

            int affectedRows;
            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    affectedRows = await conn.ExecuteAsync("SYS_USER_NOTIFICATION_CREATE", parameters, commandType: CommandType.StoredProcedure);
                    retVal = parameters.Get<int>("NOTIFICATION_ID");
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

        public int Delete(int notificationID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("NOTIFICATION_ID", notificationID, DbType.Int32);

            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = conn.Execute("SYS_USER_NOTIFICATION_DELETE", parameters, commandType: CommandType.StoredProcedure);
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

        public async Task<int> DeleteAsync(int notificationID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("USER_NOTIFICATION_ID", notificationID, DbType.Int32);

            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = await conn.ExecuteAsync("SYS_USER_NOTIFICATION_DELETE", parameters, commandType: CommandType.StoredProcedure);
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

        public int UpdateToReadTutor(int notificationID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("USER_NOTIFICATION_ID", notificationID, DbType.Int32);

            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = conn.Execute("SYS_USER_NOTIFICATION_UPDATE_TO_READ_TUTOR", parameters, commandType: CommandType.StoredProcedure);
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

        public async Task<int> UpdateToReadTutorAsync(int notificationID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("USER_NOTIFICATION_ID", notificationID, DbType.Int32);

            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = await conn.ExecuteAsync("SYS_USER_NOTIFICATION_UPDATE_TO_READ_TUTOR", parameters, commandType: CommandType.StoredProcedure);
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

        public int UpdateToReadStudent(int notificationID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("USER_NOTIFICATION_ID", notificationID, DbType.Int32);

            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = conn.Execute("SYS_USER_NOTIFICATION_UPDATE_TO_READ_STUDENT", parameters, commandType: CommandType.StoredProcedure);
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

        public async Task<int> UpdateToReadStudentAsync(int notificationID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("USER_NOTIFICATION_ID", notificationID, DbType.Int32);

            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = await conn.ExecuteAsync("SYS_USER_NOTIFICATION_UPDATE_TO_READ_STUDENT", parameters, commandType: CommandType.StoredProcedure);
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
