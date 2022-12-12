using API.Internship.Domain.Models;
using API.Internship.ResData;

namespace API.Internship.Domain.Interfaces
{
    public interface IGradeRepository : IGenericRepository<Grade>
    {
        Task<internalData> Max();
        Task<Grade> GetId(int id);
        Grade Delete(Grade obj);
    }
}
