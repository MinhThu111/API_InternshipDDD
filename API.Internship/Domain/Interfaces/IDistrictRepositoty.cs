using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Linq.Expressions;

namespace API.Internship.Domain.Interfaces
{
    public interface IDistrictRepository : IGenericRepository<District>
    {
        Task<internalData> Max();
        Task<District> GetId(int id);
        District Delete(District obj);
    }
}