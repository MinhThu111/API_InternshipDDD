using API.Internship.Domain.Interfaces;
using API.Internship.Domain.Models;
using API.Internship.Infrastructure.Data;
using API.Internship.ResData;
using Microsoft.EntityFrameworkCore;
namespace API.Internship.Infrastructure.Repositories
{
    public class ProvinceRepository : GenericRepository<Province>, IProvinceRepository
    {
        public ProvinceRepository(InternshipContext context) : base(context) { }
        public async Task<internalData> Max()
        {
            //using (db= _context)  {  }
            internalData interData = new internalData();
            try
            {
                var max = await _context.Provinces.OrderByDescending(m => m.Id).FirstOrDefaultAsync();
                interData.data = (max == null ? 0 : max.Id);

            }
            catch (Exception ex)
            {
                interData.code = -1;
                interData.message = $"Exception: {ex.Message};";
            }
            return await Task.Run(() => interData);

        }
        public async Task<Province> GetId(int id)
        {
            var query = GetAll().Where(w => w.Status != -1).AsNoTracking().FirstOrDefault(f => f.Id == id);
            return await Task.Run(() => query);
        }
        public Province Delete(Province obj)
        {
            try
            {
                obj.Status = -1; //xóa
                obj.UpdatedAt = DateTime.Now;
                obj.Timer = DateTime.Now;
                _context.Provinces.Update(obj);
            }
            catch (Exception) { }
            return obj;
        }
    }
}
