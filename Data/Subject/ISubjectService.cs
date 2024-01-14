using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mentor.Data
{
    public interface ISubjectService
    {
        int Delete(int subjectID);
        Task<int> DeleteAsync(int subjectID);
        SubjectModel Select(int subjectID);
        IEnumerable<SubjectModel> SelectAll();
        Task<IEnumerable<SubjectModel>> SelectAllAsync();
        IEnumerable<SubjectModel> SelectAllByTutor(int tutorID);
        Task<IEnumerable<SubjectModel>> SelectAllByTutorAsync(int tutorID);
        Task<SubjectModel> SelectAsync(int subjectID);
        int Update(SubjectModel subject);
        Task<int> UpdateAsync(SubjectModel subject);
    }
}