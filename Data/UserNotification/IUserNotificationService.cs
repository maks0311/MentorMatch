using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mentor.Data
{
    public interface IUserNotificationService
    {
        int Create(UserNotificationModel userNotification);
        Task<int> CreateAsync(UserNotificationModel userNotification);
        int Delete(int notificationID);
        Task<int> DeleteAsync(int notificationID);
        IEnumerable<UserNotificationModel> SelectAllByUser(int userID);
        Task<IEnumerable<UserNotificationModel>> SelectAllByUserAsync(int userID);
    }
}