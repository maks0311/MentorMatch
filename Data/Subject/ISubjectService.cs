using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mentor.Data
{
    public interface ISubjectService
    {
        IEnumerable<SubjectModel> SelectAll();
        Task<IEnumerable<SubjectModel>> SelectAllAsync();
    }
}