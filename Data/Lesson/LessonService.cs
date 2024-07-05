using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Mentor.Data
{
    public class LessonService : ILessonService
    {
        private readonly SqlConnectionConfiguration _configuration;
        public LessonService(SqlConnectionConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int Create(LessonModel lesson)
        {
            var parameters = new DynamicParameters();
            parameters.Add("LESSON_ID", lesson.LESSON_ID, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("SUBJECT_ID", lesson.SUBJECT_ID, DbType.Int32);
            parameters.Add("LEVEL_ID", lesson.LEVEL_ID, DbType.Int32);
            parameters.Add("LESSON_STATUS_ID", lesson.LESSON_STATUS_ID, DbType.Int32);
            parameters.Add("TUTOR_ID", lesson.TUTOR_ID, DbType.Int32);
            parameters.Add("STUDENT_ID", lesson.STUDENT_ID, DbType.Int32);
            parameters.Add("DATE_START", lesson.DATE_START, DbType.DateTime);
            parameters.Add("DATE_STOP", lesson.DATE_STOP, DbType.DateTime);
            parameters.Add("RATING_ID", lesson.RATING_ID, DbType.Int32);
            parameters.Add("TOPIC", lesson.TOPIC, DbType.String);

            int affectedRows;
            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    affectedRows = conn.Execute("SYS_LESSON_CREATE", parameters, commandType: CommandType.StoredProcedure);
                    retVal = parameters.Get<int>("LESSON_ID");
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

        public async Task<int> CreateAsync(LessonModel lesson)
        {
            var parameters = new DynamicParameters();
            parameters.Add("LESSON_ID", lesson.LESSON_ID, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("SUBJECT_ID", lesson.SUBJECT_ID, DbType.Int32);
            parameters.Add("LEVEL_ID", lesson.LEVEL_ID, DbType.Int32);
            parameters.Add("LESSON_STATUS_ID", lesson.LESSON_STATUS_ID, DbType.Int32);
            parameters.Add("TUTOR_ID", lesson.TUTOR_ID, DbType.Int32);
            parameters.Add("STUDENT_ID", lesson.STUDENT_ID, DbType.Int32);
            parameters.Add("DATE_START", lesson.DATE_START, DbType.DateTime);
            parameters.Add("DATE_STOP", lesson.DATE_STOP, DbType.DateTime);
            parameters.Add("RATING_ID", lesson.RATING_ID, DbType.Int32);
            parameters.Add("TOPIC", lesson.TOPIC, DbType.String);

            int affectedRows;
            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    affectedRows = await conn.ExecuteAsync("SYS_LESSON_CREATE", parameters, commandType: CommandType.StoredProcedure);
                    retVal = parameters.Get<int>("LESSON_ID");
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

        public LessonModel Select(int lessonID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("LESSON_ID", lessonID, DbType.Int32);

            LessonModel retVal = new();

            using (var conn = new SqlConnection(_configuration.Value))
            {

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = conn.QueryFirstOrDefault<LessonModel>("SYS_LESSON_SELECT", parameters, commandType: CommandType.StoredProcedure);
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

        public async Task<LessonModel> SelectAsync(int lessonID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("LESSON_ID", lessonID, DbType.Int32);

            LessonModel retVal = new();

            using (var conn = new SqlConnection(_configuration.Value))
            {

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = await conn.QueryFirstOrDefaultAsync<LessonModel>("SYS_LESSON_SELECT", parameters, commandType: CommandType.StoredProcedure);
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

        public int Update(LessonModel lesson)
        {
            int retVal = 0;

            var parameters = new DynamicParameters();
            parameters.Add("LESSON_ID", lesson.LESSON_ID, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("SUBJECT_ID", lesson.SUBJECT_ID, DbType.Int32);
            parameters.Add("LEVEL_ID", lesson.LEVEL_ID, DbType.Int32);
            parameters.Add("LESSON_STATUS_ID", lesson.LESSON_STATUS_ID, DbType.Int32);
            parameters.Add("TUTOR_ID", lesson.TUTOR_ID, DbType.Int32);
            parameters.Add("STUDENT_ID", lesson.STUDENT_ID, DbType.Int32);
            parameters.Add("DATE_START", lesson.DATE_START, DbType.DateTime);
            parameters.Add("DATE_STOP", lesson.DATE_STOP, DbType.DateTime);
            parameters.Add("RATING_ID", lesson.RATING_ID, DbType.Int32);
            parameters.Add("TOPIC", lesson.TOPIC, DbType.String);

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = conn.Execute("SYS_LESSON_UPDATE", parameters, commandType: CommandType.StoredProcedure);
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

        public async Task<int> UpdateAsync(LessonModel lesson)
        {
            int retVal = 0;

            var parameters = new DynamicParameters();
            parameters.Add("LESSON_ID", lesson.LESSON_ID, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("SUBJECT_ID", lesson.SUBJECT_ID, DbType.Int32);
            parameters.Add("LEVEL_ID", lesson.LEVEL_ID, DbType.Int32);
            parameters.Add("LESSON_STATUS_ID", lesson.LESSON_STATUS_ID, DbType.Int32);
            parameters.Add("TUTOR_ID", lesson.TUTOR_ID, DbType.Int32);
            parameters.Add("STUDENT_ID", lesson.STUDENT_ID, DbType.Int32);
            parameters.Add("DATE_START", lesson.DATE_START, DbType.DateTime);
            parameters.Add("DATE_STOP", lesson.DATE_STOP, DbType.DateTime);
            parameters.Add("RATING_ID", lesson.RATING_ID, DbType.Int32);
            parameters.Add("TOPIC", lesson.TOPIC, DbType.String);

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = await conn.ExecuteAsync("SYS_LESSON_UPDATE", parameters, commandType: CommandType.StoredProcedure);
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

        public int UpdateRating(int lesson_id, int rating_id)
        {
            int retVal = 0;

            var parameters = new DynamicParameters();
            parameters.Add("LESSON_ID", lesson_id, DbType.Int32);
            parameters.Add("RATING_ID", rating_id, DbType.Int32);

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = conn.Execute("SYS_LESSON_RATING_UPDATE", parameters, commandType: CommandType.StoredProcedure);
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

        public async Task<int> UpdateRatingAsync(int lesson_id, int rating_id)
        {
            int retVal = 0;

            var parameters = new DynamicParameters();
            parameters.Add("LESSON_ID", lesson_id, DbType.Int32);
            parameters.Add("RATING_ID", rating_id, DbType.Int32);

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = await conn.ExecuteAsync("SYS_LESSON_RATING_UPDATE", parameters, commandType: CommandType.StoredProcedure);
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

        public async Task<int> UpsertAsync(LessonModel lesson)
        {
            int retVal = 0;

            var parameters = new DynamicParameters();
            parameters.Add("LESSON_ID", lesson.LESSON_ID, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("SUBJECT_ID", lesson.SUBJECT_ID, DbType.Int32);
            parameters.Add("LEVEL_ID", lesson.LEVEL_ID, DbType.Int32);
            parameters.Add("LESSON_STATUS_ID", lesson.LESSON_STATUS_ID, DbType.Int32);
            parameters.Add("TUTOR_ID", lesson.TUTOR_ID, DbType.Int32);
            parameters.Add("STUDENT_ID", lesson.STUDENT_ID, DbType.Int32);
            parameters.Add("DATE_START", lesson.DATE_START, DbType.DateTime);
            parameters.Add("DATE_STOP", lesson.DATE_STOP, DbType.DateTime);
            parameters.Add("RATING_ID", lesson.RATING_ID, DbType.Int32);
            parameters.Add("TOPIC", lesson.TOPIC, DbType.String);

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    await conn.ExecuteAsync("SYS_LESSON_UPSERT", parameters, commandType: CommandType.StoredProcedure);
                    retVal = parameters.Get<int>("LESSON_ID");
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

        public int Upsert(LessonModel lesson)
        {
            int retVal = 0;

            var parameters = new DynamicParameters();
            parameters.Add("LESSON_ID", lesson.LESSON_ID, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("SUBJECT_ID", lesson.SUBJECT_ID, DbType.Int32);
            parameters.Add("LEVEL_ID", lesson.LEVEL_ID, DbType.Int32);
            parameters.Add("LESSON_STATUS_ID", lesson.LESSON_STATUS_ID, DbType.Int32);
            parameters.Add("TUTOR_ID", lesson.TUTOR_ID, DbType.Int32);
            parameters.Add("STUDENT_ID", lesson.STUDENT_ID, DbType.Int32);
            parameters.Add("DATE_START", lesson.DATE_START, DbType.DateTime);
            parameters.Add("DATE_STOP", lesson.DATE_STOP, DbType.DateTime);
            parameters.Add("RATING_ID", lesson.RATING_ID, DbType.Int32);
            parameters.Add("TOPIC", lesson.TOPIC, DbType.String);

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    conn.Execute("SYS_LESSON_UPSERT", parameters, commandType: CommandType.StoredProcedure);
                    retVal = parameters.Get<int>("LESSON_ID");
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

        public IEnumerable<LessonModel> SelectAll()
        {
            var parameters = new DynamicParameters();

            IEnumerable<LessonModel> lessonInfoList;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    lessonInfoList = conn.Query<LessonModel>("SYS_LESSON_SELECT_ALL", parameters, commandType: CommandType.StoredProcedure);
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
            return lessonInfoList;

        }

        public async Task<IEnumerable<LessonModel>> SelectAllAsync()
        {
            var parameters = new DynamicParameters();

            IEnumerable<LessonModel> lessonInfoList;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    lessonInfoList = await conn.QueryAsync<LessonModel>("SYS_LESSON_SELECT_ALL", parameters, commandType: CommandType.StoredProcedure);
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
            return lessonInfoList;

        }

        public IEnumerable<LessonModel> SelectAllByTutor(int tutorID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("TUTOR_ID", tutorID, DbType.Int32);

            IEnumerable<LessonModel> lessonInfoList;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    lessonInfoList = conn.Query<LessonModel>("SYS_LESSON_SELECT_ALL_BY_TUTOR", parameters, commandType: CommandType.StoredProcedure);
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
            return lessonInfoList;
        }

        public async Task<IEnumerable<LessonModel>> SelectAllByTutorAsync(int tutorID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("TUTOR_ID", tutorID, DbType.Int32);

            IEnumerable<LessonModel> lessonInfoList;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    lessonInfoList = await conn.QueryAsync<LessonModel>("SYS_LESSON_SELECT_ALL_BY_TUTOR", parameters, commandType: CommandType.StoredProcedure);
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
            return lessonInfoList;
        }

        public IEnumerable<LessonModel> SelectAllByStudent(int studentID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("STUDENT_ID", studentID, DbType.Int32);

            IEnumerable<LessonModel> lessonInfoList;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    lessonInfoList = conn.Query<LessonModel>("SYS_LESSON_SELECT_ALL_BY_STUDENT", parameters, commandType: CommandType.StoredProcedure);
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
            return lessonInfoList;
        }

        public async Task<IEnumerable<LessonModel>> SelectAllByStudentAsync(int studentID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("STUDENT_ID", studentID, DbType.Int32);

            IEnumerable<LessonModel> lessonInfoList;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    lessonInfoList = await conn.QueryAsync<LessonModel>("SYS_LESSON_SELECT_ALL_BY_STUDENT", parameters, commandType: CommandType.StoredProcedure);
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
            return lessonInfoList;
        }

        public IEnumerable<LessonModel> SelectArchiveByTutor(int tutorID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("TUTOR_ID", tutorID, DbType.Int32);

            IEnumerable<LessonModel> lessonInfoList;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    lessonInfoList = conn.Query<LessonModel>("SYS_LESSON_SELECT_ARCHIVE_BY_TUTOR", parameters, commandType: CommandType.StoredProcedure);
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
            return lessonInfoList;
        }

        public async Task<IEnumerable<LessonModel>> SelectArchiveByTutorAsync(int tutorID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("TUTOR_ID", tutorID, DbType.Int32);

            IEnumerable<LessonModel> lessonInfoList;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    lessonInfoList = await conn.QueryAsync<LessonModel>("SYS_LESSON_SELECT_ARCHIVE_BY_TUTOR", parameters, commandType: CommandType.StoredProcedure);
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
            return lessonInfoList;
        }

        public IEnumerable<LessonModel> SelectArchiveByStudent(int studentID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("STUDENT_ID", studentID, DbType.Int32);

            IEnumerable<LessonModel> lessonInfoList;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    lessonInfoList = conn.Query<LessonModel>("SYS_LESSON_SELECT_ARCHIVE_BY_STUDENT", parameters, commandType: CommandType.StoredProcedure);
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
            return lessonInfoList;
        }

        public async Task<IEnumerable<LessonModel>> SelectArchiveByStudentAsync(int studentID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("STUDENT_ID", studentID, DbType.Int32);

            IEnumerable<LessonModel> lessonInfoList;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    lessonInfoList = await conn.QueryAsync<LessonModel>("SYS_LESSON_SELECT_ARCHIVE_BY_STUDENT", parameters, commandType: CommandType.StoredProcedure);
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
            return lessonInfoList;
        }
    }
}
