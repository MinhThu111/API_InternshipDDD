using API.Internship.Domain.Interfaces;
using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Linq.Expressions;
namespace API.Internship.Domain.Services
{
    public interface IDistrictService
    {
        Task<R_Data> GetAsync(int id);
        Task<R_Data> GetListAsync(Expression<Func<District, bool>> expression);
        Task<R_Data> Delete(int id, int? updatedBy);
        Task<R_Data> PutAsync(int id, string name, string nameslug, string districtcode, int provinceid, DateTime timer, int? updateby);
        Task<R_Data> PutAsync(string name, string nameslug, string districtcode, int provinceid);
    }
    public class DistrictService : IDistrictService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DistrictService> _logger;
        public DistrictService(IUnitOfWork unitOfWork, ILogger<DistrictService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<R_Data> GetAsync(int id)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<District>(new District());
            try
            {
                categoryObj = await _unitOfWork.DistrictRepository.GetId(id);
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
        public async Task<R_Data> GetListAsync(Expression<Func<District, bool>> expression)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var lstObj = await Task.FromResult<List<District>>(new List<District>());
            try
            {
                lstObj = (await _unitOfWork.DistrictRepository.ListAsync(expression)).ToList();
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
        public async Task<R_Data> Delete(int id, int? updatedBy)
        {
            error errObj = new error();
            R_Data res = new R_Data() { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<District>(new District());
            try
            {
                Expression<Func<District, bool>> filter;
                filter = a => a.Id == id;
                categoryObj = _unitOfWork.DistrictRepository.Find(filter); if (categoryObj == null)
                {
                    res.result = 0;
                    errObj.message = $"Không tìm thấy {id} để xóa.";
                }
                else
                {
                    //categoryObj.UpdatedBy = categoryObj.UpdatedBy;
                    _unitOfWork.DistrictRepository.Delete(categoryObj);
                    var result = await _unitOfWork.CommitAsync();
                    if (result > 0)
                    {
                        categoryObj = await _unitOfWork.DistrictRepository.GetId(id);
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
        public async Task<R_Data> PutAsync(int id, string name, string nameslug, string districtcode, int provinceid, DateTime timer, int? updateby)
        {
            error errObj = new error();
            R_Data res = new R_Data() { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<District>(new District());


            var existingGrade = await _unitOfWork.DistrictRepository.GetId(id);
            //var existingGrade = new InternshipContext().Grades.FirstOrDefault(f => f.Id == id);
            if (existingGrade == null)
                throw new Exception($"District {id} không tìm thấy.");

            if (existingGrade.Timer > timer)
            {
                res.result = 0;
                res.data = null;
                res.error = new error() { code = 201, message = "Thông tin đã được cập nhật lại trước đó. Vui lòng hủy thao tác và thực hiện lại để dữ liệu đồng bộ!" };
                return res;
            }

            District item = new District()
            {
                Name = name,
                NameSlug = nameslug,
                DistrictCode = districtcode,
                ProvinceId = provinceid,
                UpdatedBy = updateby,
                UpdatedAt = DateTime.Now
            };

            try
            {
                await _unitOfWork.DistrictRepository.UpdateAsync(item);
                //await _unitOfWork.DistrictRepository.UpdateAsync(item);
                var result = await _unitOfWork.CommitAsync();
                if (result > 0)
                {
                    categoryObj = await _unitOfWork.DistrictRepository.GetId(item.Id);
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
        public async Task<R_Data> PutAsync(string name, string nameslug, string districtcode, int provinceid)
        {
            error errObj = new error();
            R_Data res = new R_Data() { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<District>(new District());


            var idMax = await _unitOfWork.DistrictRepository.Max();
            if (idMax.code != 1)
                throw new Exception();

            District item = new District()
            {
                Id = idMax.data + 1,
                Name = name,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Timer = DateTime.Now,
                Status = 1,
                DistrictCode = districtcode,
                ProvinceId = provinceid,
                NameSlug = nameslug,

            };

            try
            {
                _unitOfWork.DistrictRepository.Add(item);
                var result = await _unitOfWork.CommitAsync();
                if (result > 0)
                {
                    categoryObj = await _unitOfWork.DistrictRepository.GetId(item.Id);
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
