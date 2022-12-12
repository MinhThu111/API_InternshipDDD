using API.Internship.Domain.Interfaces;
using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Linq.Expressions;
namespace API.Internship.Domain.Services
{
    public interface INationalityService
    {
        Task<R_Data> GetAsync(int id);
        Task<R_Data> GetListAsync(Expression<Func<Nationality, bool>> expression);
        Task<R_Data> Delete(int id, int? updatedBy);
        Task<R_Data> PutAsync(int id, string name, string nameslug, string description, int? updateby, DateTime timer);
        Task<R_Data> PutAsync(string name, string nameslug, string description);
    }
    public class NationalityService : INationalityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<NationalityService> _logger;
        public NationalityService(IUnitOfWork unitOfWork, ILogger<NationalityService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<R_Data> GetAsync(int id)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<Nationality>(new Nationality());
            try
            {
                categoryObj = await _unitOfWork.NationalityRepository.GetId(id);
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
        public async Task<R_Data> GetListAsync(Expression<Func<Nationality, bool>> expression)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var lstObj = await Task.FromResult<List<Nationality>>(new List<Nationality>());
            try
            {
                lstObj = (await _unitOfWork.NationalityRepository.ListAsync(expression)).ToList();
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
            var categoryObj = await Task.FromResult<Nationality>(new Nationality());
            try
            {
                Expression<Func<Nationality, bool>> filter;
                filter = a => a.Id == id;
                categoryObj = _unitOfWork.NationalityRepository.Find(filter); if (categoryObj == null)
                {
                    res.result = 0;
                    errObj.message = $"Không tìm thấy {id} để xóa.";
                }
                else
                {
                    categoryObj.UpdatedBy = categoryObj.UpdatedBy;
                    _unitOfWork.NationalityRepository.Delete(categoryObj);
                    var result = await _unitOfWork.CommitAsync();
                    if (result > 0)
                    {
                        categoryObj = await _unitOfWork.NationalityRepository.GetId(id);
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
            return res;
        }
        public async Task<R_Data> PutAsync(int id, string name, string nameslug, string description, int? updateby, DateTime timer)
        {
            error errObj = new error();
            R_Data res = new R_Data() { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<Nationality>(new Nationality());


            var existingGrade = await _unitOfWork.NationalityRepository.GetId(id);
            //var existingGrade = new InternshipContext().Grades.FirstOrDefault(f => f.Id == id);
            if (existingGrade == null)
                throw new Exception($"Nationality {id} không tìm thấy.");

            if (existingGrade.Timer > timer)
            {
                res.result = 0;
                res.data = null;
                res.error = new error() { code = 201, message = "Thông tin đã được cập nhật lại trước đó. Vui lòng hủy thao tác và thực hiện lại để dữ liệu đồng bộ!" };
                return res;
            }

            Nationality item = new Nationality()
            {
                Name = name,
                NameSlug = nameslug,
                Description = description,
                UpdatedBy = updateby,
                UpdatedAt = DateTime.Now
            };

            try
            {
                await _unitOfWork.NationalityRepository.UpdateAsync(item);
                //await _unitOfWork.NationalityRepository.UpdateAsync(item);
                var result = await _unitOfWork.CommitAsync();
                if (result > 0)
                {
                    categoryObj = await _unitOfWork.NationalityRepository.GetId(item.Id);
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
        public async Task<R_Data> PutAsync(string name, string nameslug, string description)
        {
            error errObj = new error();
            R_Data res = new R_Data() { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<Nationality>(new Nationality());


            var idMax = await _unitOfWork.NationalityRepository.Max();
            if (idMax.code != 1)
                throw new Exception();

            Nationality item = new Nationality()
            {
                Id= idMax.data+1,
                Name = name,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Timer = DateTime.Now,
                Status = 1,
                NameSlug = nameslug,
                Description = description

            };

            try
            {
                _unitOfWork.NationalityRepository.Add(item);
                var result = await _unitOfWork.CommitAsync();
                if (result > 0)
                {
                    categoryObj = await _unitOfWork.NationalityRepository.GetId(item.Id);
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
