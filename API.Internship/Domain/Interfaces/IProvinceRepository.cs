using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Linq.Expressions;

namespace API.Internship.Domain.Interfaces
{
    public interface IProvinceRepository: IGenericRepository<Province>
    {
        Task<internalData> Max();
        Task<Province> GetId(int id);
        Province Delete(Province obj);
    }
}
