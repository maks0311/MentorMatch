using Dapper;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Mentor.Data
{
    public class SubjectService : ISubjectService
    {
        private static NLog.ILogger AppLogger = NLog.LogManager.GetCurrentClassLogger();

        private readonly SqlConnectionConfiguration _configuration;
        public SubjectService(SqlConnectionConfiguration configuration)
        {
            _configuration = configuration;
        }


        public int Create(SubjectModel subject)
        {
            var parameters = new DynamicParameters();
            parameters.Add("SUBJECT_ID", subject.SUBJECT_ID, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("SUBJECT_NAME", subject.SUBJECT_NAME, DbType.String);
            parameters.Add("IS_ACTIVE", subject.IS_ACTIVE, DbType.Boolean);

            int affectedRows;
            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    affectedRows = conn.Execute("SYS_SUBJECT_CREATE", parameters, commandType: CommandType.StoredProcedure);
                    retVal = parameters.Get<int>("SUBJECT_ID");
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

        public async Task<int> CreateAsync(SubjectModel subject)
        {
            var parameters = new DynamicParameters();
            parameters.Add("SUBJECT_ID", subject.SUBJECT_ID, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("SUBJECT_NAME", subject.SUBJECT_NAME, DbType.String);
            parameters.Add("IS_ACTIVE", subject.IS_ACTIVE, DbType.Boolean);

            int affectedRows;
            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    affectedRows = await conn.ExecuteAsync("SYS_SUBJECT_CREATE", parameters, commandType: CommandType.StoredProcedure);
                    retVal = parameters.Get<int>("SUBJECT_ID");
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

        public async Task<IEnumerable<SubjectModel>> SelectAllAsync()
        {
            var parameters = new DynamicParameters();

            IEnumerable<SubjectModel> SubjectEnum;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    SubjectEnum = await conn.QueryAsync<SubjectModel>("SYS_SUBJECT_SELECT_ALL", parameters, commandType: CommandType.StoredProcedure);
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
            return SubjectEnum;
        }

        public IEnumerable<SubjectModel> SelectAll()
        {
            var parameters = new DynamicParameters();

            IEnumerable<SubjectModel> SubjectEnum;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    SubjectEnum = conn.Query<SubjectModel>("SYS_SUBJECT_SELECT_ALL", parameters, commandType: CommandType.StoredProcedure);
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
            return SubjectEnum;
        }

        public IEnumerable<SubjectModel> SelectAllByTutor(int tutorID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("TUTOR_ID", tutorID, DbType.Int32);

            IEnumerable<SubjectModel> SubjectEnum;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    SubjectEnum = conn.Query<SubjectModel>("SYS_SUBJECT_SELECT_ALL_BY_TUTOR", parameters, commandType: CommandType.StoredProcedure);
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
            return SubjectEnum;
        }
        public async Task<IEnumerable<SubjectModel>> SelectAllByTutorAsync(int tutorID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("TUTOR_ID", tutorID, DbType.Int32);

            IEnumerable<SubjectModel> SubjectEnum;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    SubjectEnum = await conn.QueryAsync<SubjectModel>("SYS_SUBJECT_SELECT_ALL_BY_TUTOR", parameters, commandType: CommandType.StoredProcedure);
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
            return SubjectEnum;
        }

        public async Task<SubjectModel> SelectAsync(int subjectID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("SUBJECT_ID", subjectID, DbType.Int32);

            SubjectModel subject;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    subject = await conn.QueryFirstOrDefaultAsync<SubjectModel>("SYS_SUBJECT_SELECT", parameters, commandType: CommandType.StoredProcedure);
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
            return subject;
        }

        public SubjectModel Select(int subjectID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("SUBJECT_ID", subjectID, DbType.Int32);

            SubjectModel subject;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    subject = conn.QueryFirstOrDefault<SubjectModel>("SYS_SUBJECT_SELECT", parameters, commandType: CommandType.StoredProcedure);
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
            return subject;
        }

        public int Update(SubjectModel subject)
        {
            int retVal = 0;

            var parameters = new DynamicParameters();
            parameters.Add("SUBJECT_ID", subject.SUBJECT_ID, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("SUBJECT_NAME", subject.SUBJECT_NAME, DbType.String);
            parameters.Add("IS_ACTIVE", subject.IS_ACTIVE, DbType.Boolean);

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = conn.Execute("SYS_SUBJECT_UPDATE", parameters, commandType: CommandType.StoredProcedure);
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

        public async Task<int> UpdateAsync(SubjectModel subject)
        {
            int retVal = 0;

            var parameters = new DynamicParameters();
            parameters.Add("SUBJECT_ID", subject.SUBJECT_ID, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("SUBJECT_NAME", subject.SUBJECT_NAME, DbType.String);
            parameters.Add("IS_ACTIVE", subject.IS_ACTIVE, DbType.Boolean);

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = await conn.ExecuteAsync("SYS_SUBJECT_UPDATE", parameters, commandType: CommandType.StoredProcedure);
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

        public int Delete(int subjectID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("SUBJECT_ID", subjectID, DbType.Int32);

            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = conn.Execute("SYS_SUBJECT_DELETE", parameters, commandType: CommandType.StoredProcedure);
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

        public async Task<int> DeleteAsync(int subjectID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("SUBJECT_ID", subjectID, DbType.Int32);

            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = await conn.ExecuteAsync("SYS_SUBJECT_DELETE", parameters, commandType: CommandType.StoredProcedure);
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
