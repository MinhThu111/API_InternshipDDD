using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Linq.Expressions;
namespace API.Internship.Domain.Interfaces
{
    public interface IParentContactRepository: IGenericRepository<ParentContact>
    {
        Task<internalData> Max();
        Task<ParentContact> GetId(int id);
        ParentContact Delete(ParentContact obj);
    }
}
