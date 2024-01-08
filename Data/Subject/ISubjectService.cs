using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mentor.Data
{
    public interface ISubjectService
    {
        IEnumerable<SubjectModel> SelectAll();
        Task<IEnumerable<SubjectModel>> SelectAllAsync();
        IEnumerable<SubjectModel> SelectAllByTutor(int tutorID);
        Task<IEnumerable<SubjectModel>> SelectAllByTutorAsync(int tutorID);
    }
}