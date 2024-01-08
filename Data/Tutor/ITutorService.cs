using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mentor.Data
{
    public interface ITutorService
    {
        IEnumerable<TutorModel> SearchAll(string tutorName, int subjectID, int levelID);
        Task<IEnumerable<TutorModel>> SearchAllAsync(string tutorName, int subjectID, int levelID);
        TutorModel Select(int tutorID);
        IEnumerable<TutorModel> SelectAll();
        Task<IEnumerable<TutorModel>> SelectAllAsync();
        IEnumerable<TutorModel> SelectAllByCompetence(int subjectID, int levelID);
        Task<IEnumerable<TutorModel>> SelectAllByCompetenceAsync(int subjectID, int levelID);
        Task<TutorModel> SelectAsync(int tutorID);
    }
}