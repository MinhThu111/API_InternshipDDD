using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Linq.Expressions;
namespace API.Internship.Domain.Interfaces
{
    public interface ITeacherSubjectRepository: IGenericRepository<TeacherSubject>
    {
        Task<internalData> Max();
        Task<TeacherSubject> GetId(int id);
        TeacherSubject Delete(TeacherSubject obj);
    }
}
