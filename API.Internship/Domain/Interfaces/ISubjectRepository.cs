using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Linq.Expressions;
namespace API.Internship.Domain.Interfaces
{
    public interface ISubjectRepository: IGenericRepository<Subject>
    {
        Task<internalData> Max();
        Task<Subject> GetId(int id);
        Subject Delete(Subject obj);
    }
}
