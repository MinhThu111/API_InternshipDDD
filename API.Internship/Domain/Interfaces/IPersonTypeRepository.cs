using API.Internship.Domain.Models;
using API.Internship.ResData;

namespace API.Internship.Domain.Interfaces
{
    public interface IPersonTypeRepository: IGenericRepository<PersonType>
    {
        Task<internalData> Max();
        Task<PersonType> GetId(int id);
        PersonType Delete(PersonType obj);
    }
}
