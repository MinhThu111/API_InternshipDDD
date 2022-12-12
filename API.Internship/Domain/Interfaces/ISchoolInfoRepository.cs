using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Linq.Expressions;
namespace API.Internship.Domain.Interfaces
{
    public interface ISchoolInfoRepository: IGenericRepository<SchoolInfo>
    {
        Task<internalData> Max();
        Task<SchoolInfo> GetId(int id);
        SchoolInfo Delete(SchoolInfo obj);
    }
}
