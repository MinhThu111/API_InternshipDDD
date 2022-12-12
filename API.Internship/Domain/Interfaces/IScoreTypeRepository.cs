using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Linq.Expressions;
namespace API.Internship.Domain.Interfaces
{
    public interface IScoreTypeRepository: IGenericRepository<ScoreType>
    {
        Task<internalData> Max();
        Task<ScoreType> GetId(int id);
        ScoreType Delete(ScoreType obj);
    }
}
