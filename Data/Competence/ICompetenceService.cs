using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mentor.Data
{
    public interface ICompetenceService
    {
        int Create(CompetenceModel competence);
        Task<int> CreateAsync(CompetenceModel competence);
        int Delete(int competenceID);
        Task<int> DeleteAsync(int competenceID);
        IEnumerable<CompetenceModel> SelectAllByTutor(int tutorID);
        Task<IEnumerable<CompetenceModel>> SelectAllByTutorAsync(int tutorID);
    }
}