using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Linq.Expressions;

namespace API.Internship.Domain.Interfaces
{
    public interface IAddressRepository: IGenericRepository<Address>
    {
        Task<internalData> Max();
        Task<Address> GetId(int id);
        Address Delete(Address obj);
    }
}
