using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mentor.Data
{
    public interface IUserService
    {
        int Authenticate(string userName, string userPass);
        int Create(UserModel user);
        Task<int> CreateAsync(UserModel user);
        int Delete(int userID);
        Task<int> DeleteAsync(int userID);
        int PasswordUpdate(int userID, string pass);
        Task<int> PasswordUpdateAsync(int userID, string pass);
        UserModel Select(int userID);
        IEnumerable<UserModel> SelectAll();
        Task<IEnumerable<UserModel>> SelectAllAsync();
        Task<UserModel> SelectAsync(int userID);
        UserModel SelectByEmail(string userEmail);
        Task<UserModel> SelectByEmailAsync(string userEmail);
        int Update(UserModel user);
        Task<int> UpdateAsync(UserModel user);
    }
}