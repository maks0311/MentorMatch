using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mentor.Data
{
    public interface IRatingService
    {
        IEnumerable<RatingModel> SelectAll();
        Task<IEnumerable<RatingModel>> SelectAllAsync();
    }
}