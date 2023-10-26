using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mentor.Data
{
    public interface IWorkTypeService
    {
        IEnumerable<WorkTypeModel> SelectAll();
        Task<IEnumerable<WorkTypeModel>> SelectAllAsync();
    }
}