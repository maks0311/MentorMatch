using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mentor.Data
{
    public interface ICityService
    {
        int Create(CityModel city);
        Task<int> CreateAsync(CityModel city);
        int Delete(int cityID);
        Task<int> DeleteAsync(int cityID);
        CityModel Select(int cityID);
        IEnumerable<CityModel> SelectAll();
        Task<IEnumerable<CityModel>> SelectAllAsync();
        Task<CityModel> SelectAsync(int cityID);
        int Update(CityModel city);
        Task<int> UpdateAsync(CityModel city);
    }
}