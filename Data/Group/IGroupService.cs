using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mentor.Data
{
    public interface IGroupService
    {
        int Create(GroupModel group);
        Task<int> CreateAsync(GroupModel group);
        IEnumerable<GroupModel> SelectAll();
        Task<IEnumerable<GroupModel>> SelectAllAsync();
    }
}