using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mentor.Data
{
    public interface IAvailabilityService
    {
        int Create(int tutorID, DateTime start, DateTime stop);
        Task<int> CreateAsync(int tutorID, DateTime start, DateTime stop);
        int Delete(int tutorID, DateTime start, DateTime end);
        Task<int> DeleteAsync(int tutorID, DateTime start, DateTime end);
        IEnumerable<AvailabilityModel> SelectAll(int tutorID, DateTime start, DateTime end);
        Task<IEnumerable<AvailabilityModel>> SelectAllAsync(int tutorID, DateTime start, DateTime end);
    }
}