using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Linq.Expressions;

namespace API.Internship.Domain.Interfaces
{
    public interface INationalityRepository : IGenericRepository<Nationality>
    {
        Task<internalData> Max();
        Task<Nationality> GetId(int id);
        Nationality Delete(Nationality obj);
    }
}
