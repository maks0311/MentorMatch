using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mentor.Data
{
    public interface ILevelService
    {
        int Delete(int levelID);
        Task<int> DeleteAsync(int lessonID);
        LevelModel Select(int leveID);
        IEnumerable<LevelModel> SelectAll();
        Task<IEnumerable<LevelModel>> SelectAllAsync();
        IEnumerable<LevelModel> SelectAllByTutor(int tutorID);
        Task<IEnumerable<LevelModel>> SelectAllByTutorAsync(int tutorID);
        Task<LevelModel> SelectAsync(int leveID);
        int Update(LevelModel level);
        Task<int> UpdateAsync(LevelModel level);
    }
}