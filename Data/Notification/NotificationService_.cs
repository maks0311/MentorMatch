using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;
using System;
using System.Diagnostics;


namespace Mentor.Data
{
    public class NotificationService_ : INotificationService_
    {
        private readonly SqlConnectionConfiguration _configuration;
        public NotificationService_(SqlConnectionConfiguration configuration)
        {
            _configuration = configuration;
        }

        public NotificationModel Select(int notificationID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("NOTIFICATION_ID", notificationID, DbType.Int32);

            NotificationModel retVal = new();

            using (var conn = new SqlConnection(_configuration.Value))
            {

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = conn.QueryFirstOrDefault<NotificationModel>("SYS_NOTIFICATION_SELECT", parameters, commandType: CommandType.StoredProcedure);
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

        public async Task<NotificationModel> SelectAsync(int notificationID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("NOTIFICATION_ID", notificationID, DbType.Int32);

            NotificationModel retVal = new();

            using (var conn = new SqlConnection(_configuration.Value))
            {

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = await conn.QueryFirstOrDefaultAsync<NotificationModel>("SYS_USER_SELECT", parameters, commandType: CommandType.StoredProcedure);
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
