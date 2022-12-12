using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Linq.Expressions;

namespace API.Internship.Domain.Interfaces
{
    public interface ICountryRepository: IGenericRepository<Country>
    {
        Task<internalData> Max();
        Task<Country> GetId(int id);
        Country Delete(Country obj);
    }
}
