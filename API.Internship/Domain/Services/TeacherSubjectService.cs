using API.Internship.Domain.Interfaces;
using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Linq.Expressions;
namespace API.Internship.Domain.Services
{
    public interface ITeacherSubjectService
    {
        Task<R_Data> GetAsync(int id);
        Task<R_Data> GetListAsync(Expression<Func<TeacherSubject, bool>> expression);
        Task<R_Data> PutAsync(int id, int? teacherid, int? subjectid, int? updateby, DateTime timer);
        Task<R_Data> PutAsync(int? teacherid, int? subjectid, string remark);
        Task<R_Data> Delete(int id, int? updatedBy);
    }
    public class TeacherSubjectService: ITeacherSubjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TeacherSubjectService> _logger;
        public TeacherSubjectService(IUnitOfWork unitOfWork, ILogger<TeacherSubjectService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<R_Data> GetAsync(int id)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<TeacherSubject>(new TeacherSubject());
            try
            {
                categoryObj = await _unitOfWork.TeacherSubjectRepository.GetId(id);
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
        public async Task<R_Data> GetListAsync(Expression<Func<TeacherSubject, bool>> expression)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var lstObj = await Task.FromResult<List<TeacherSubject>>(new List<TeacherSubject>());
            try
            {
                lstObj = (await _unitOfWork.TeacherSubjectRepository.ListAsync(expression)).ToList();
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
        public async Task<R_Data> Delete(int id, int? updateby)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<TeacherSubject>(new TeacherSubject());
            try
            {
                Expression<Func<TeacherSubject, bool>> filter;
                filter = w => w.Id == id;
                categoryObj = _unitOfWork.TeacherSubjectRepository.Find(filter);
                if (categoryObj == null)
                {
                    res.result = 0;
                    errObj.message = $"Không tìm thấy {id} để xóa.";
                }
                else
                {
                    _unitOfWork.TeacherSubjectRepository.Delete(categoryObj);
                    var result = await _unitOfWork.CommitAsync();
                    if (result > 0)
                    {
                        categoryObj = await _unitOfWork.TeacherSubjectRepository.GetId(id);
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

        public async Task<R_Data> PutAsync(int id,int? teacherid, int? subjectid, int? updateby, DateTime timer)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<TeacherSubject>(new TeacherSubject());
            var existTeacherSubject = await _unitOfWork.TeacherSubjectRepository.GetId(id);
            if (existTeacherSubject == null)
            {
                throw new Exception($"Grade {id} không tìm thấy.");
            }
            if (existTeacherSubject.Timer > timer)
            {
                res.result = 0;
                res.data = null;
                res.error = new error() { code = 201, message = "Thông tin đã được cập nhật lại trước đó. Vui lòng hủy thao tác và thực hiện lại để dữ liệu đồng bộ!" };
                return res;
            }
            TeacherSubject item = new TeacherSubject()
            {
                TeacherId = teacherid,
                SubjectId = subjectid,
                UpdatedBy = updateby,
                UpdatedAt = DateTime.Now
            };
            try
            {
                await _unitOfWork.TeacherSubjectRepository.UpdateAsync(item);
                var result = await _unitOfWork.CommitAsync();
                if (result > 0)
                {
                    categoryObj = await _unitOfWork.TeacherSubjectRepository.GetId(item.Id);
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

        public async Task<R_Data> PutAsync(int? teacherid, int? subjectid, string remark)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<TeacherSubject>(new TeacherSubject());
            var idMax = await _unitOfWork.TeacherSubjectRepository.Max();
            if (idMax.code != 1)
                throw new Exception();

            TeacherSubject item = new TeacherSubject()
            {
                Id = idMax.data + 1,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Timer = DateTime.Now,
                Status = 1,
                TeacherId = teacherid,
                SubjectId = subjectid,
                Remark = remark

            };

            try
            {
                _unitOfWork.TeacherSubjectRepository.Add(item);
                var result = await _unitOfWork.CommitAsync();
                if (result > 0)
                {
                    categoryObj = await _unitOfWork.TeacherSubjectRepository.GetId(item.Id);
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
    }
}
