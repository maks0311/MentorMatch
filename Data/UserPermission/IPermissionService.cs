using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mentor.Data
{
    public interface IPermissionService
    {
        int Create(PermissionModel permission);
        Task<int> CreateAsync(PermissionModel permission);
        IEnumerable<PermissionModel> Select(int UserGrouoID);
        Task<IEnumerable<PermissionModel>> SelectAsync(int UserGrouoID);
        int Update(PermissionModel permission);
        Task<int> UpdateAsync(PermissionModel permission);
    }
}