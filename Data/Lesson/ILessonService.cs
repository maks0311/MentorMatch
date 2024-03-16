using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mentor.Data
{
    public interface ILessonService
    {
        int Create(LessonModel lesson);
        Task<int> CreateAsync(LessonModel lesson);
        int Delete(int lessonID);
        Task<int> DeleteAsync(int lessonID);
        LessonModel Select(int lessonID);
        IEnumerable<LessonModel> SelectAll();
        Task<IEnumerable<LessonModel>> SelectAllAsync();
        IEnumerable<LessonModel> SelectAllByStudent(int studentID);
        Task<IEnumerable<LessonModel>> SelectAllByStudentAsync(int studentID);
        IEnumerable<LessonModel> SelectAllByTutor(int tutorID);
        Task<IEnumerable<LessonModel>> SelectAllByTutorAsync(int tutorID);
        Task<LessonModel> SelectAsync(int lessonID);
        int Update(LessonModel lesson);
        Task<int> UpdateAsync(LessonModel lesson);
        int UpdateRating(int lesson_id, int rating_id);
        Task<int> UpdateRatingAsync(int lesson_id, int rating_id);
        int Upsert(LessonModel lesson);
        Task<int> UpsertAsync(LessonModel lesson);
    }
}