using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Linq.Expressions;

namespace API.Internship.Domain.Interfaces
{
    public interface IReligionRepository : IGenericRepository<Religion>
    {
        Task<internalData> Max();
        Task<Religion> GetId(int id);
        Religion Delete(Religion obj);
    }
}
