using Dapper;
using Mentor.Data;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System;
using Mentor;

public class CityService : ICityService
{
    private static NLog.ILogger AppLogger = NLog.LogManager.GetCurrentClassLogger();

    private readonly SqlConnectionConfiguration _configuration;
    public CityService(SqlConnectionConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IEnumerable<CityModel> SelectAll()
    {
        var parameters = new DynamicParameters();
        IEnumerable<CityModel> cityInfoList;

        using (var conn = new SqlConnection(_configuration.Value))
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                cityInfoList = conn.Query<CityModel>("SYS_CITY_SELECT_ALL", parameters, commandType: CommandType.StoredProcedure);
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
        return cityInfoList;
    }

    public async Task<IEnumerable<CityModel>> SelectAllAsync()
    {
        var parameters = new DynamicParameters();
        IEnumerable<CityModel> cityInfoList;

        using (var conn = new SqlConnection(_configuration.Value))
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                cityInfoList = await conn.QueryAsync<CityModel>("SYS_CITY_SELECT_ALL", parameters, commandType: CommandType.StoredProcedure);
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
        return cityInfoList;
    }

    public IEnumerable<CityModel> SelectAllByTutor(int tutorID)
    {
        var parameters = new DynamicParameters();
        parameters.Add("TUTOR_ID", tutorID, DbType.Int32);

        IEnumerable<CityModel> cityInfoList;

        using (var conn = new SqlConnection(_configuration.Value))
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                cityInfoList = conn.Query<CityModel>("SYS_CITY_SELECT_ALL_BY_TUTOR", parameters, commandType: CommandType.StoredProcedure);
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
        return cityInfoList;
    }

    public async Task<IEnumerable<CityModel>> SelectAllByTutorAsync(int tutorID)
    {
        var parameters = new DynamicParameters();
        parameters.Add("TUTOR_ID", tutorID, DbType.Int32);

        IEnumerable<CityModel> cityInfoList;

        using (var conn = new SqlConnection(_configuration.Value))
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                cityInfoList = await conn.QueryAsync<CityModel>("SYS_CITY_SELECT_ALL_BY_TUTOR", parameters, commandType: CommandType.StoredProcedure);
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
        return cityInfoList;
    }
}
