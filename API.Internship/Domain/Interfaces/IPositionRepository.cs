using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Linq.Expressions;
namespace API.Internship.Domain.Interfaces
{
    public interface IPositionRepository: IGenericRepository<Position>
    {
        Task<internalData> Max();
        Task<Position> GetId(int id);
        Position Delete(Position obj);
    }
}
