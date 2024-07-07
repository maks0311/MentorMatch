using Dapper;
using Mentor;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mentor.Data
{
    public class CompetenceService : ICompetenceService
    {

        private readonly SqlConnectionConfiguration _configuration;
        public CompetenceService(SqlConnectionConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int Create(CompetenceModel competence)
        {
            var parameters = new DynamicParameters();
            parameters.Add("ID", competence.ID, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("TUTOR_ID", competence.TUTOR_ID, DbType.Int32);
            parameters.Add("SUBJECT_ID", competence.SUBJECT_ID, DbType.Int32);
            parameters.Add("LEVEL_ID", competence.LEVEL_ID, DbType.Int32);
            parameters.Add("DESCRIPTION", competence.DESCRIPTION, DbType.String);

            int affectedRows;
            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    affectedRows = conn.Execute("SYS_COMPETENCE_CREATE", parameters, commandType: CommandType.StoredProcedure);
                    retVal = parameters.Get<int>("ID");
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

        public async Task<int> CreateAsync(CompetenceModel competence)
        {
            var parameters = new DynamicParameters();
            parameters.Add("ID", competence.ID, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("TUTOR_ID", competence.TUTOR_ID, DbType.Int32);
            parameters.Add("SUBJECT_ID", competence.SUBJECT_ID, DbType.Int32);
            parameters.Add("LEVEL_ID", competence.LEVEL_ID, DbType.Int32);
            parameters.Add("DESCRIPTION", competence.DESCRIPTION, DbType.String);

            int affectedRows;
            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    affectedRows = await conn.ExecuteAsync("SYS_COMPETENCE_CREATE", parameters, commandType: CommandType.StoredProcedure);
                    retVal = parameters.Get<int>("ID");
                }
                catch (Exception ex)
                {
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

        public int Delete(int competenceID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("ID", competenceID, DbType.Int32);

            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = conn.Execute("SYS_COMPETENCE_DELETE", parameters, commandType: CommandType.StoredProcedure);
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

        public async Task<int> DeleteAsync(int competenceID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("ID", competenceID, DbType.Int32);

            int retVal = 0;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    retVal = await conn.ExecuteAsync("SYS_COMPETENCE_DELETE", parameters, commandType: CommandType.StoredProcedure);
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

        public IEnumerable<CompetenceModel> SelectAllByTutor(int tutorID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("TUTOR_ID", tutorID, DbType.Int32);

            IEnumerable<CompetenceModel> competenceList;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    competenceList = conn.Query<CompetenceModel>("SYS_COMPETENCE_SELECT_ALL_BY_TUTOR", parameters, commandType: CommandType.StoredProcedure);
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
            return competenceList;
        }

        public async Task<IEnumerable<CompetenceModel>> SelectAllByTutorAsync(int tutorID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("TUTOR_ID", tutorID, DbType.Int32);

            IEnumerable<CompetenceModel> competenceList;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    competenceList = await conn.QueryAsync<CompetenceModel>("SYS_COMPETENCE_SELECT_ALL_BY_TUTOR", parameters, commandType: CommandType.StoredProcedure);
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
            return competenceList;
        }
    }
}
