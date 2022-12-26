using API.Internship.Domain.Interfaces;
using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Linq.Expressions;
namespace API.Internship.Domain.Services
{
    public interface IProvinceService
    {
        Task<R_Data> GetAsync(int id);
        Task<R_Data> GetListAsync(Expression<Func<Province, bool>> expression);
        Task<R_Data> GetListAsync(int? countryId);
        Task<R_Data> Delete(int id, int? updatedBy);
        Task<R_Data> PutAsync(int id, string name, string nameslug, int? countryid, string provincecode, DateTime timer);
        Task<R_Data> PutAsync(string name, string nameslug, int? countryid, string provincecode);
    }
    public class ProvinceSerivce : IProvinceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProvinceSerivce> _logger;
        public ProvinceSerivce(IUnitOfWork unitOfWork, ILogger<ProvinceSerivce> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<R_Data> GetAsync(int id)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<Province>(new Province());
            try
            {
                categoryObj = await _unitOfWork.ProvinceRepository.GetId(id);
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
        public async Task<R_Data> GetListAsync(Expression<Func<Province, bool>> expression)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var lstObj = await Task.FromResult<List<Province>>(new List<Province>());
            try
            {
                lstObj = (await _unitOfWork.ProvinceRepository.ListAsync(expression)).ToList();
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
        public async Task<R_Data> GetListAsync(int? countryId)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var lstObj = await Task.FromResult<List<Province>>(new List<Province>());
            try
            {
                Expression<Func<Province, bool>> filter;
                filter = w => w.Status == 1;
                filter.Compile();
                lstObj = (await _unitOfWork.ProvinceRepository.ListAsync(filter)).ToList();

                var datas = lstObj.Where(x => x.CountryId == 1).Select(x => new
                {
                    id = x.Id,
                    name = x.Name
                });
                if (lstObj == null)
                {
                    errObj.message = "Load data is successful and do not data to show!";
                }
                else
                {
                    res.data = datas;
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
        public async Task<R_Data>Delete(int id, int? updatedBy)
        {
            error errObj = new error();
            R_Data res = new R_Data() { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<Province>(new Province());
            try
            {
                Expression<Func<Province, bool>> filter;
                filter = a => a.Id == id;
                categoryObj = _unitOfWork.ProvinceRepository.Find(filter); if (categoryObj == null)
                {
                    res.result = 0;
                    errObj.message = $"Không tìm thấy {id} để xóa.";
                }
                else
                {
                    categoryObj.UpdatedBy = categoryObj.UpdatedBy;
                    _unitOfWork.ProvinceRepository.Delete(categoryObj);
                    var result = await _unitOfWork.CommitAsync();
                    if (result > 0)
                    {
                        categoryObj = await _unitOfWork.ProvinceRepository.GetId(id);
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
        public async Task<R_Data> PutAsync(int id, string name, string nameslug, int? countryid, string provincecode,DateTime timer)
        {
            error errObj = new error();
            R_Data res = new R_Data() { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<Province>(new Province());


            var existingGrade = await _unitOfWork.ProvinceRepository.GetId(id);
            //var existingGrade = new InternshipContext().Grades.FirstOrDefault(f => f.Id == id);
            if (existingGrade == null)
                throw new Exception($"Province {id} không tìm thấy.");

            if (existingGrade.Timer > timer)
            {
                res.result = 0;
                res.data = null;
                res.error = new error() { code = 201, message = "Thông tin đã được cập nhật lại trước đó. Vui lòng hủy thao tác và thực hiện lại để dữ liệu đồng bộ!" };
                return res;
            }

            Province item = new Province()
            {
                Name = name,
                NameSlug = nameslug,
                CountryId = countryid,
                ProvinceCode = provincecode,
                UpdatedAt = DateTime.Now
            };

            try
            {
                await _unitOfWork.ProvinceRepository.UpdateAsync(item);
                //await _unitOfWork.ProvinceRepository.UpdateAsync(item);
                var result = await _unitOfWork.CommitAsync();
                if (result > 0)
                {
                    categoryObj = await _unitOfWork.ProvinceRepository.GetId(item.Id);
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
            return await Task.Run(()=>res);
        }
        public async Task<R_Data> PutAsync(string name, string nameslug, int? countryid, string provincecode)
        {
            error errObj = new error();
            R_Data res = new R_Data() { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<Province>(new Province());


            var idMax = await _unitOfWork.ProvinceRepository.Max();
            if (idMax.code != 1)
                throw new Exception();

            Province item = new Province()
            {
                Id=idMax.data+1,
                Name = name,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Timer = DateTime.Now,
                Status = 1,
                NameSlug = nameslug,

            };

            try
            {
                _unitOfWork.ProvinceRepository.Add(item);
                var result = await _unitOfWork.CommitAsync();
                if (result > 0)
                {
                    categoryObj = await _unitOfWork.ProvinceRepository.GetId(item.Id);
                    errObj.message = "Thêm dữ liệu thành công.";
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
