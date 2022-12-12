using API.Internship.Domain.Interfaces;
using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Linq.Expressions;
namespace API.Internship.Domain.Services
{
    public interface IStudentParentContactService
    {
        Task<R_Data> GetAsync(int id);
        Task<R_Data> GetListAsync(Expression<Func<StudentParentContact, bool>> expression);
        Task<R_Data> PutAsync(int id, int studentid, int parentcontactid, DateTime? timer);
        Task<R_Data> PutAsync(int studentid, int parentcontactid);
        Task<R_Data> Delete(int id, int? updatedBy);
    }
    public class StudentParentContactService: IStudentParentContactService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<StudentParentContactService> _logger;
        public StudentParentContactService(IUnitOfWork unitOfWork, ILogger<StudentParentContactService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<R_Data> GetAsync(int id)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<StudentParentContact>(new StudentParentContact());
            try
            {
                categoryObj = await _unitOfWork.StudentParentContactRepository.GetId(id);
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
        public async Task<R_Data> GetListAsync(Expression<Func<StudentParentContact, bool>> expression)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var lstObj = await Task.FromResult<List<StudentParentContact>>(new List<StudentParentContact>());
            try
            {
                lstObj = (await _unitOfWork.StudentParentContactRepository.ListAsync(expression)).ToList();
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
            var categoryObj = await Task.FromResult<StudentParentContact>(new StudentParentContact());
            try
            {
                Expression<Func<StudentParentContact, bool>> filter;
                filter = w => w.Id == id;
                categoryObj = _unitOfWork.StudentParentContactRepository.Find(filter);
                if (categoryObj == null)
                {
                    res.result = 0;
                    errObj.message = $"Không tìm thấy {id} để xóa.";
                }
                else
                {
                    _unitOfWork.StudentParentContactRepository.Delete(categoryObj);
                    var result = await _unitOfWork.CommitAsync();
                    if (result > 0)
                    {
                        categoryObj = await _unitOfWork.StudentParentContactRepository.GetId(id);
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

        public async Task<R_Data> PutAsync(int id, int studentid, int parentcontactid, DateTime? timer)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<StudentParentContact>(new StudentParentContact());
            var existStudentParentContact = await _unitOfWork.StudentParentContactRepository.GetId(id);
            if (existStudentParentContact == null)
            {
                throw new Exception($"Grade {id} không tìm thấy.");
            }
            if (existStudentParentContact.Timer > timer)
            {
                res.result = 0;
                res.data = null;
                res.error = new error() { code = 201, message = "Thông tin đã được cập nhật lại trước đó. Vui lòng hủy thao tác và thực hiện lại để dữ liệu đồng bộ!" };
                return res;
            }
            StudentParentContact item = new StudentParentContact()
            {
                StudentId = studentid,
                ParentContactId = parentcontactid,
                UpdatedAt = DateTime.Now
            };
            try
            {
                await _unitOfWork.StudentParentContactRepository.UpdateAsync(item);
                var result = await _unitOfWork.CommitAsync();
                if (result > 0)
                {
                    categoryObj = await _unitOfWork.StudentParentContactRepository.GetId(item.Id);
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

        public async Task<R_Data> PutAsync(int studentid, int parentcontactid)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<StudentParentContact>(new StudentParentContact());
            var idMax = await _unitOfWork.StudentParentContactRepository.Max();
            if (idMax.code != 1)
                throw new Exception();

            StudentParentContact item = new StudentParentContact()
            {
                Id = idMax.data + 1,
                StudentId = studentid,
                ParentContactId = parentcontactid,
                Timer = DateTime.Now,
                CreatedAt = DateTime.Now

            };

            try
            {
                _unitOfWork.StudentParentContactRepository.Add(item);
                var result = await _unitOfWork.CommitAsync();
                if (result > 0)
                {
                    categoryObj = await _unitOfWork.StudentParentContactRepository.GetId(item.Id);
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
