using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Linq.Expressions;
namespace API.Internship.Domain.Interfaces
{
    public interface IScoreRepository: IGenericRepository<Score>
    {
        Task<internalData> Max();
        Task<Score> GetId(int id);
        Score Delete(Score obj);
    }
}
