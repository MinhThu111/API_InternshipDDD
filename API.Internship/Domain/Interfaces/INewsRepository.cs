using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Linq.Expressions;
namespace API.Internship.Domain.Interfaces
{
    public interface INewsRepository: IGenericRepository<News>
    {
        Task<internalData> Max();
        Task<News> GetId(int id);
        News Delete(News obj);
    }
}
