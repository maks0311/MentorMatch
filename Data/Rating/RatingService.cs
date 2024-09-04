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
    public class RatingService : IRatingService
    {
        private readonly SqlConnectionConfiguration _configuration;
        public RatingService(SqlConnectionConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<RatingModel>> SelectAllAsync()
        {
            var parameters = new DynamicParameters();

            IEnumerable<RatingModel> RatingEnum;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    RatingEnum = await conn.QueryAsync<RatingModel>("SYS_RATING_SELECT_ALL", parameters, commandType: CommandType.StoredProcedure);
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
            return RatingEnum;
        }

        public IEnumerable<RatingModel> SelectAll()
        {
            var parameters = new DynamicParameters();

            IEnumerable<RatingModel> RatingEnum;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    RatingEnum = conn.Query<RatingModel>("SYS_RATING_SELECT_ALL", parameters, commandType: CommandType.StoredProcedure);
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
            return RatingEnum;
        }
    }
}
