using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mentor.Data
{
    public interface ILessonStatusService
    {
        int Create(LessonStatusModel lesson);
        Task<int> CreateAsync(LessonStatusModel lesson);
        int Delete(int statusID);
        Task<int> DeleteAsync(int statusID);
        LessonModel Select(int statusID);
        IEnumerable<LessonStatusModel> SelectAll();
        Task<IEnumerable<LessonStatusModel>> SelectAllAsync();
        Task<LessonStatusModel> SelectAsync(int statusID);
        int Update(LessonStatusModel status);
        Task<int> UpdateAsync(LessonStatusModel status);
    }
}