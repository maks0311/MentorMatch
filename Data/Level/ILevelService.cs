using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mentor.Data
{
    public interface ILevelService
    {
        int Create(LevelModel level);
        Task<int> CreateAsync(LevelModel level);
        int Delete(int levelID);
        Task<int> DeleteAsync(int lessonID);
        LevelModel Select(int levelID);
        IEnumerable<LevelModel> SelectAll();
        Task<IEnumerable<LevelModel>> SelectAllAsync();
        IEnumerable<LevelModel> SelectAllByTutor(int tutorID);
        Task<IEnumerable<LevelModel>> SelectAllByTutorAsync(int tutorID);
        Task<LevelModel> SelectAsync(int levelID);
        int Update(LevelModel level);
        Task<int> UpdateAsync(LevelModel level);
    }
}