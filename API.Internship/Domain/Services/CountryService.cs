using API.Internship.Domain.Interfaces;
using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Linq.Expressions;
namespace API.Internship.Domain.Services
{
    public interface ICountryService
    {
        Task<R_Data> GetAsync(int id);
        Task<R_Data> GetListAsync(Expression<Func<Country, bool>> expression);
        Task<R_Data> Delete(int id, int? updatedBy);
        Task<R_Data> PutAsync(int id, string name, string remark, int? updateby, DateTime timer);
        Task<R_Data> PutAsync(string name);
        Task<R_Data> PutAsync(int id, int? status, int? updatedBy, DateTime timer);
    }
    public class CountryService: ICountryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CountryService> _logger;
        public CountryService(IUnitOfWork unitOfWork, ILogger<CountryService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<R_Data> GetAsync(int id)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<Country>(new Country());
            try
            {
                categoryObj = await _unitOfWork.CountryRepository.GetId(id);
                if (categoryObj == null)
                {
                    errObj.message = "Load data is successful and do not data to show!";
                }
                else
                {
                    res.data = categoryObj;
                }
            }
            catch (Exception ex)
            {
                res.result = 0;
                res.data = null;
                res.error = new error { code = 201, message = $"Exception: Xẩy ra lỗi khi đọc dữ liệu {ex}" };
            }
            return res;
        }
        public async Task<R_Data> GetListAsync(Expression<Func<Country, bool>> expression)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var lstObj = await Task.FromResult<List<Country>>(new List<Country>());
            try
            {
                lstObj = (await _unitOfWork.CountryRepository.ListAsync(expression)).ToList();
                if (lstObj == null)
                {
                    errObj.message = "Load data is successful and do not data to show!";
                }
                else
                {
                    res.data = lstObj;
                }
            }
            catch (Exception ex)
            {
                res.result = 0;
                res.data = null;
                res.error = new error { code = 201, message = $"Exception: Xẩy ra lỗi khi đọc dữ liệu {ex}" };
            }
            return res;
        }
        public async Task<R_Data>Delete(int id, int?updateby)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<Country>(new Country());
            try
            {
                Expression<Func<Country, bool>> filter;
                filter = w => w.Id == id;
                categoryObj = _unitOfWork.CountryRepository.Find(filter);
                if(categoryObj == null)
                {
                    res.result = 0;
                    errObj.message = $"Không tìm thấy {id} để xóa.";
                }
                else
                {
                    _unitOfWork.CountryRepository.Delete(categoryObj);
                    var result = await _unitOfWork.CommitAsync();
                    if(result>0)
                    {
                        categoryObj = await _unitOfWork.CountryRepository.GetId(id);
                        if (categoryObj == null)
                            errObj.message = "Đã xóa dữ liệu thành công.";
                    }
                }
                res.data = categoryObj;
            }
            catch (Exception ex)
            {
                res.result = 0;
                res.data = null;
                res.error = new error { code = 201, message = $"Exception: Xẩy ra lỗi khi xóa dữ liệu {ex}" };
            }
            return await Task.Run(() => res);
        }

        public async Task<R_Data> PutAsync(int id, string name, string remark, int? updateby, DateTime timer)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<Country>(new Country());
            var existCountry = await _unitOfWork.CountryRepository.GetId(id);
            if (existCountry == null)
            {
                throw new Exception($"Grade {id} không tìm thấy.");
            }
            if (existCountry.Timer > timer)
            {
                res.result = 0;
                res.data = null;
                res.error = new error() { code = 201, message = "Thông tin đã được cập nhật lại trước đó. Vui lòng hủy thao tác và thực hiện lại để dữ liệu đồng bộ!" };
                return res;
            }
            Country item = new Country()
            {
                Id=id,
                Name = name,
                Remark = remark,
                UpdatedBy = updateby,
                NameSlug = existCountry.NameSlug,
                CountryCode = existCountry.CountryCode,
                CreatedAt = existCountry.CreatedAt,
                CreatedBy = existCountry.CreatedBy,
                UpdatedAt = DateTime.Now,
                Status = existCountry.Status,
                Timer = DateTime.Now,
            };
            try
            {
                await _unitOfWork.CountryRepository.UpdateAsync(item);
                var result = await _unitOfWork.CommitAsync();
                if (result > 0)
                {
                    categoryObj = await _unitOfWork.CountryRepository.GetId(item.Id);
                    errObj.message = "Cập nhật dữ liệu thành công.";
                }
                res.data = categoryObj;
            }
            catch (Exception ex)
            {
                res.result = 0;
                res.data = null;
                res.error = new error { code = 201, message = $"Exception: Xẩy ra lỗi khi cập nhật dữ liệu {ex}" };
            }

            return await Task.Run(() => res);
        }

        public async Task<R_Data> PutAsync(string name)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<Country>(new Country());
            var idMax = await _unitOfWork.CountryRepository.Max();
            if (idMax.code != 1)
                throw new Exception();

            Country item = new Country()
            {
                Id=idMax.data+1,
                Name = name,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Timer = DateTime.Now,
                Status = 1,

            };

            try
            {
                _unitOfWork.CountryRepository.Add(item);
                var result = await _unitOfWork.CommitAsync();
                if (result > 0)
                {
                    categoryObj = await _unitOfWork.CountryRepository.GetId(item.Id);
                    errObj.message = "Thêm dữ liệu thành công.";
                }
                res.data = categoryObj;

            }
            catch (Exception ex)
            {
                res.result = 0;
                res.data = null;
                res.error = new error() { code = -1, message = $"Exeception: {ex.Message}" };
            }


            return await Task.Run(() => res);
        }
        public async Task<R_Data> PutAsync(int id, int? status, int? updatedBy, DateTime timer)
        {
            error errObj = new error();
            R_Data res = new R_Data() { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<Country>(new Country());


            var existingCountry = await _unitOfWork.CountryRepository.GetId(id);
            //var existingPerson = new InternshipContext().Persons.FirstOrDefault(f => f.Id == id);
            if (existingCountry == null)
                throw new Exception($"Person {id} không tìm thấy.");

            if (existingCountry.Timer > timer)
            {
                res.result = 0;
                res.data = null;
                res.error = new error() { code = 201, message = "Thông tin đã được cập nhật lại trước đó. Vui lòng hủy thao tác và thực hiện lại để dữ liệu đồng bộ!" };
                return res;
            }

            Country item = new Country
            {
                Id = existingCountry.Id,
                Name=existingCountry.Name,
                NameSlug=existingCountry.Name,
                CountryCode=existingCountry.CountryCode,
                Remark=existingCountry.Remark,
                CreatedAt = existingCountry.CreatedAt,
                CreatedBy = existingCountry.CreatedBy,
                UpdatedAt = DateTime.Now,
                UpdatedBy = updatedBy,
                Status = status,
                Timer = DateTime.Now,
            };

            try
            {
                await _unitOfWork.CountryRepository.UpdateAsync(item);
                //await _unitOfWork.PersonRepository.UpdateAsync(item);
                var result = await _unitOfWork.CommitAsync();
                if (result > 0)
                {
                    categoryObj = await _unitOfWork.CountryRepository.GetId(item.Id);
                    errObj.message = "Cập nhật dữ liệu thành công.";
                }
                res.data = categoryObj;
            }
            catch (Exception ex)
            {
                res.result = 0;
                res.data = null;
                res.error = new error { code = 201, message = $"Exception: Xẩy ra lỗi khi thêm dữ liệu {ex}" };

            }
            return res;
        }
    }
}
