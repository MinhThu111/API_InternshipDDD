using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Linq.Expressions;

namespace API.Internship.Domain.Interfaces
{
    public interface IFolkRepository : IGenericRepository<Folk>
    {
        Task<internalData> Max();
        Task<Folk> GetId(int id);
        Folk Delete(Folk obj);
    }
}
