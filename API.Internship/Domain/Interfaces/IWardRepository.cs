using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Linq.Expressions;

namespace API.Internship.Domain.Interfaces
{
    public interface IWardRepository : IGenericRepository<Ward>
    {
        Task<internalData> Max();
        Task<Ward> GetId(int id);
        Ward Delete(Ward obj);
    }
}
