using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mentor.Data
{
    public interface IUserToCityService
    {
        int Create(UserToCityModel userToCity);
        Task<int> CreateAsync(UserToCityModel userToCity);
        int Delete(int userToCityID);
        Task<int> DeleteAsync(int userToCityID);
        IEnumerable<UserToCityModel> SelectAllByTutor(int tutorID);
        Task<IEnumerable<UserToCityModel>> SelectAllByTutorAsync(int tutorID);
    }
}