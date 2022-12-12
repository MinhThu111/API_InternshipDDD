using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Linq.Expressions;
namespace API.Internship.Domain.Interfaces
{
    public interface IStudentParentContactRepository: IGenericRepository<StudentParentContact>
    {
        Task<internalData> Max();
        Task<StudentParentContact> GetId(int id);
        StudentParentContact Delete(StudentParentContact obj);
    }
}
