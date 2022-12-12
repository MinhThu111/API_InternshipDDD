using API.Internship.Domain.Models;
using API.Internship.ResData;

namespace API.Internship.Domain.Interfaces
{
    public interface IPersonRepository : IGenericRepository<Person>
    {
        Task<internalData> Max();
        Task<Person> GetId(int id);
        Person Delete(Person obj);
    }
}
