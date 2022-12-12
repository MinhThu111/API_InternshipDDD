using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Linq.Expressions;
namespace API.Internship.Domain.Interfaces
{
    public interface INewsCategoryRepository: IGenericRepository<NewsCategory>
    {
        Task<internalData> Max();
        Task<NewsCategory> GetId(int id);
        NewsCategory Delete(NewsCategory obj);
    }
}
