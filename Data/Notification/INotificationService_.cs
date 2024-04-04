using System.Threading.Tasks;

namespace Mentor.Data
{
    public interface INotificationService_
    {
        NotificationModel Select(int notificationID);
        Task<NotificationModel> SelectAsync(int notificationID);
    }
}