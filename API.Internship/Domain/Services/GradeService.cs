using API.Internship.Domain.Interfaces;
using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Linq.Expressions;

namespace API.Internship.Domain.Services
{
    public interface IGradeService
    {
        Task<R_Data> GetAsync(int id);
        Task<R_Data> GetListAsync(Expression<Func<Grade, bool>> expression);
        Task<R_Data> PutAsync(string name, string classCode, int? teacherId, string remark, int? createdBy);
        Task<R_Data> PutAsync(int id, string name, string classCode, int? teacherId, string remark, int? status, int? updatedBy, DateTime timer);
        Task<R_Data> PutAsync(int id, int? status, int? updatedBy, DateTime timer);
        Task<R_Data> Delete(int id, int? updatedBy);
    }
    public class GradeService : IGradeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GradeService> _logger;
        public GradeService(IUnitOfWork unitOfWork, ILogger<GradeService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<R_Data> GetAsync(int id)
        {
            error errObj = new error();
            R_Data res = new R_Data() { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<Grade>(new Grade());
            try
            {
                categoryObj = await _unitOfWork.GradeRepository.GetId(id);
                if (categoryObj == null)
                    errObj.message = "Load data is successful and do not data to show!";
                else
                    res.data = categoryObj;
            }
            catch (Exception ex)
            {
                res.result = 0;
                res.data = null;
                res.error = new error { code = 201, message = $"Exception: Xẩy ra lỗi khi đọc dữ liệu {ex}" };
            }
            return res;
        }
        public async Task<R_Data> GetListAsync(Expression<Func<Grade, bool>> expression)
        {
            error errObj = new error();
            R_Data res = new R_Data() { result = 1, data = null, error = errObj };
            var lstGrades = await Task.FromResult<List<Grade>>(new List<Grade>());
            try
            {
                lstGrades = (await _unitOfWork.GradeRepository.ListAsync(expression)).ToList();
                if (lstGrades.Count <= 0)
                    errObj.message = "Load data is successful and do not data to show!";
                else
                    res.data = lstGrades;
            }
            catch (Exception ex)
            {
                res.result = 0;
                res.data = null;
                res.error = new error { code = 201, message = $"Exception: Xẩy ra lỗi khi đọc dữ liệu {ex}" };
            }
            return res;

        }
        public async Task<R_Data> PutAsync(string name, string classCode, int? teacherId, string remark, int? createdBy)
        {
            error errObj = new error();
            R_Data res = new R_Data() { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<Grade>(new Grade());


            var idMax = await _unitOfWork.GradeRepository.Max();
            if (idMax.code != 1)
                throw new Exception();

            Grade item = new Grade
            {
                Id = idMax.data + 1,
                Name = name,
                GradeCode = classCode,
                Remark = remark,
                TeacherId = teacherId,
                CreatedAt = DateTime.Now,
                CreatedBy = createdBy,
                UpdatedAt = DateTime.Now,
                UpdatedBy = createdBy,
                Status = 1,
                Timer = DateTime.Now,
            };

            try
            {
                _unitOfWork.GradeRepository.Add(item);
                var result = await _unitOfWork.CommitAsync();
                if (result > 0)
                {
                    categoryObj = await _unitOfWork.GradeRepository.GetId(item.Id);
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
        public async Task<R_Data> PutAsync(int id, string name, string classCode, int? teacherId, string remark, int? status, int? updatedBy, DateTime timer)
        {
            error errObj = new error();
            R_Data res = new R_Data() { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<Grade>(new Grade());


            var existingGrade = await _unitOfWork.GradeRepository.GetId(id);
            //var existingGrade = new InternshipContext().Grades.FirstOrDefault(f => f.Id == id);
            if (existingGrade == null)
                throw new Exception($"Grade {id} không tìm thấy.");

            if (existingGrade.Timer > timer)
            {
                res.result = 0;
                res.data = null;
                res.error = new error() { code = 201, message = "Thông tin đã được cập nhật lại trước đó. Vui lòng hủy thao tác và thực hiện lại để dữ liệu đồng bộ!" };
                return res;
            }

            Grade item = new Grade
            {
                Id = existingGrade.Id,
                Name = name,
                GradeCode = classCode,
                Remark = remark,
                TeacherId = teacherId,
                CreatedAt = DateTime.Now,
                CreatedBy = existingGrade.CreatedBy,
                UpdatedAt = DateTime.Now,
                UpdatedBy = updatedBy,
                Status = status,
                Timer = DateTime.Now,
            };

            try
            {
                await _unitOfWork.GradeRepository.UpdateAsync(item);
                //await _unitOfWork.GradeRepository.UpdateAsync(item);
                var result = await _unitOfWork.CommitAsync();
                if (result > 0)
                {
                    categoryObj = await _unitOfWork.GradeRepository.GetId(item.Id);
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
            return res;
        }
        public async Task<R_Data> PutAsync(int id, int? status, int? updatedBy, DateTime timer)
        {
            error errObj = new error();
            R_Data res = new R_Data() { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<Grade>(new Grade());


            var existingGrade = await _unitOfWork.GradeRepository.GetId(id);
            //var existingGrade = new InternshipContext().Grades.FirstOrDefault(f => f.Id == id);
            if (existingGrade == null)
                throw new Exception($"Grade {id} không tìm thấy.");

            if (existingGrade.Timer > timer)
            {
                res.result = 0;
                res.data = null;
                res.error = new error() { code = 201, message = "Thông tin đã được cập nhật lại trước đó. Vui lòng hủy thao tác và thực hiện lại để dữ liệu đồng bộ!" };
                return res;
            }

            Grade item = new Grade
            {
                Id = existingGrade.Id,
                Name = existingGrade.Name,
                GradeCode = existingGrade.GradeCode,
                Remark = existingGrade.Remark,
                TeacherId = existingGrade.TeacherId,
                CreatedAt = DateTime.Now,
                CreatedBy = existingGrade.CreatedBy,
                UpdatedAt = DateTime.Now,
                UpdatedBy = updatedBy,
                Status = status,
                Timer = DateTime.Now,
            };

            try
            {
                await _unitOfWork.GradeRepository.UpdateAsync(item);
                //await _unitOfWork.GradeRepository.UpdateAsync(item);
                var result = await _unitOfWork.CommitAsync();
                if (result > 0)
                {
                    categoryObj = await _unitOfWork.GradeRepository.GetId(item.Id);
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
        public async Task<R_Data> Delete(int id, int? updatedBy)
        {
            error errObj = new error();
            R_Data res = new R_Data() { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<Grade>(new Grade());
            try
            {
                Expression<Func<Grade, bool>> filter;
                filter = a => a.Id == id;
                categoryObj = _unitOfWork.GradeRepository.Find(filter); if (categoryObj == null)
                {
                    res.result = 0;
                    errObj.message = $"Không tìm thấy {id} để xóa.";
                }
                else
                {
                    categoryObj.UpdatedBy = categoryObj.UpdatedBy;
                    _unitOfWork.GradeRepository.Delete(categoryObj);
                    var result = await _unitOfWork.CommitAsync();
                    if (result > 0)
                    {
                        categoryObj = await _unitOfWork.GradeRepository.GetId(id);
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
    }
}
