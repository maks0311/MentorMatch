using Mentor.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ICityService
{
    IEnumerable<CityModel> SelectAll();
    Task<IEnumerable<CityModel>> SelectAllAsync();
    IEnumerable<CityModel> SelectAllByTutor(int tutorID);
    Task<IEnumerable<CityModel>> SelectAllByTutorAsync(int tutorID);
}