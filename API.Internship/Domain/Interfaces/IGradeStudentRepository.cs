using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Linq.Expressions;
namespace API.Internship.Domain.Interfaces
{
    public interface IGradeStudentRepository: IGenericRepository<GradeStudent>
    {
        Task<internalData> Max();
        Task<GradeStudent> GetId(int id);
        GradeStudent Delete(GradeStudent obj);
    }
}
