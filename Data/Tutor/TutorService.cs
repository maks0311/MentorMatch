using Dapper;
using Mentor;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Mentor.Pages;
using System.Diagnostics;

namespace Mentor.Data
{
    public class TutorService : ITutorService
    {
        private readonly SqlConnectionConfiguration _configuration;
        public TutorService(SqlConnectionConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TutorModel Select(int tutorID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("TUTOR_ID", tutorID, DbType.Int32);

            TutorModel retVal = new();

            using (var conn = new SqlConnection(_configuration.Value))
            {

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = conn.QueryFirstOrDefault<TutorModel>("SYS_TUTOR_SELECT", parameters, commandType: CommandType.StoredProcedure);
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

        public async Task<TutorModel> SelectAsync(int tutorID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("TUTOR_ID", tutorID, DbType.Int32);

            TutorModel retVal = new();

            using (var conn = new SqlConnection(_configuration.Value))
            {

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = await conn.QueryFirstOrDefaultAsync<TutorModel>("SYS_TUTOR_SELECT", parameters, commandType: CommandType.StoredProcedure);
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

        public IEnumerable<TutorModel> SelectAll()
        {
            var parameters = new DynamicParameters();
            IEnumerable<TutorModel> tutorInfoList;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    tutorInfoList = conn.Query<TutorModel>("SYS_TUTOR_SELECT_ALL", parameters, commandType: CommandType.StoredProcedure);
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
            return tutorInfoList;
        }

        public async Task<IEnumerable<TutorModel>> SelectAllAsync()
        {
            var parameters = new DynamicParameters();
            IEnumerable<TutorModel> tutorInfoList;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    tutorInfoList = await conn.QueryAsync<TutorModel>("SYS_TUTOR_SELECT_ALL", parameters, commandType: CommandType.StoredProcedure);
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
            return tutorInfoList;
        }

        public IEnumerable<TutorModel> SelectAllByCompetence(int subjectID, int levelID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("SUBJECT_ID", subjectID, DbType.Int32);
            parameters.Add("LEVEL_ID", levelID, DbType.Int32);

            IEnumerable<TutorModel> tutorInfoList;

            using (var conn = new SqlConnection(_configuration.Value))
            {

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    tutorInfoList = conn.Query<TutorModel>("SYS_TUTOR_SELECT_BY_COMPETENCE", parameters, commandType: CommandType.StoredProcedure);
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
            return tutorInfoList;
        }

        public async Task<IEnumerable<TutorModel>> SelectAllByCompetenceAsync(int subjectID, int levelID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("SUBJECT_ID", subjectID, DbType.Int32);
            parameters.Add("LEVEL_ID", levelID, DbType.Int32);

            IEnumerable<TutorModel> tutorInfoList;

            using (var conn = new SqlConnection(_configuration.Value))
            {

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    tutorInfoList = await conn.QueryAsync<TutorModel>("SYS_TUTOR_SELECT_BY_COMPETENCE", parameters, commandType: CommandType.StoredProcedure);
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
            return tutorInfoList;
        }
        public IEnumerable<TutorModel> SearchAll(string tutorName, int subjectID, int levelID, int worktypeID, int cityID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("TUTOR_NAME", tutorName, DbType.String);
            parameters.Add("SUBJECT_ID", subjectID, DbType.Int32);
            parameters.Add("LEVEL_ID", levelID, DbType.Int32);
            parameters.Add("WORK_TYPE_ID", worktypeID, DbType.Int32);
            parameters.Add("CITY_ID", cityID, DbType.Int32);

            IEnumerable<TutorModel> tutorInfoList;

            using (var conn = new SqlConnection(_configuration.Value))
            {

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    tutorInfoList = conn.Query<TutorModel>("SYS_TUTOR_SEARCH_ALL", parameters, commandType: CommandType.StoredProcedure);
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
            return tutorInfoList;
        }

        public async Task<IEnumerable<TutorModel>> SearchAllAsync(string tutorName, int subjectID, int levelID, int worktypeID, int cityID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("TUTOR_NAME", tutorName, DbType.String);
            parameters.Add("SUBJECT_ID", subjectID, DbType.Int32);
            parameters.Add("LEVEL_ID", levelID, DbType.Int32);
            parameters.Add("WORK_TYPE_ID", worktypeID, DbType.Int32);
            parameters.Add("CITY_ID", cityID, DbType.Int32);

            IEnumerable<TutorModel> tutorInfoList;

            using (var conn = new SqlConnection(_configuration.Value))
            {

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    tutorInfoList = await conn.QueryAsync<TutorModel>("SYS_TUTOR_SEARCH_ALL", parameters, commandType: CommandType.StoredProcedure);
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
            return tutorInfoList;
        }
    }
}
