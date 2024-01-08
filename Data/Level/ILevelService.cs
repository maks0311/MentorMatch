using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mentor.Data
{
    public interface ILevelService
    {
        IEnumerable<LevelModel> SelectAll();
        Task<IEnumerable<LevelModel>> SelectAllAsync();
        IEnumerable<LevelModel> SelectAllByTutor(int tutorID);
        Task<IEnumerable<LevelModel>> SelectAllByTutorAsync(int tutorID);
    }
}