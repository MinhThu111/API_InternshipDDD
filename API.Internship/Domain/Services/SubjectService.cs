using API.Internship.Domain.Interfaces;
using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Linq.Expressions;
namespace API.Internship.Domain.Services
{
    public interface ISubjectService
    {
        Task<R_Data> GetAsync(int id);
        Task<R_Data> GetListAsync(Expression<Func<Subject, bool>> expression);
        Task<R_Data> PutAsync(int id, string name, string subjectcode, string description, int? updateby, DateTime timer);
        Task<R_Data> PutAsync(string name, string subjectcode, string description);
        Task<R_Data> Delete(int id, int? updatedBy);
    }
    public class SubjectService: ISubjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SubjectService> _logger;
        public SubjectService(IUnitOfWork unitOfWork, ILogger<SubjectService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<R_Data> GetAsync(int id)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<Subject>(new Subject());
            try
            {
                categoryObj = await _unitOfWork.SubjectRepository.GetId(id);
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
        public async Task<R_Data> GetListAsync(Expression<Func<Subject, bool>> expression)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var lstObj = await Task.FromResult<List<Subject>>(new List<Subject>());
            try
            {
                lstObj = (await _unitOfWork.SubjectRepository.ListAsync(expression)).ToList();
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
            var categoryObj = await Task.FromResult<Subject>(new Subject());
            try
            {
                Expression<Func<Subject, bool>> filter;
                filter = w => w.Id == id;
                categoryObj = _unitOfWork.SubjectRepository.Find(filter);
                if (categoryObj == null)
                {
                    res.result = 0;
                    errObj.message = $"Không tìm thấy {id} để xóa.";
                }
                else
                {
                    _unitOfWork.SubjectRepository.Delete(categoryObj);
                    var result = await _unitOfWork.CommitAsync();
                    if (result > 0)
                    {
                        categoryObj = await _unitOfWork.SubjectRepository.GetId(id);
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

        public async Task<R_Data> PutAsync(int id, string name, string subjectcode, string description, int? updateby, DateTime timer)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<Subject>(new Subject());
            var existSubject = await _unitOfWork.SubjectRepository.GetId(id);
            if (existSubject == null)
            {
                throw new Exception($"Grade {id} không tìm thấy.");
            }
            if (existSubject.Timer > timer)
            {
                res.result = 0;
                res.data = null;
                res.error = new error() { code = 201, message = "Thông tin đã được cập nhật lại trước đó. Vui lòng hủy thao tác và thực hiện lại để dữ liệu đồng bộ!" };
                return res;
            }
            Subject item = new Subject()
            {
                Name = name,
                SubjectCode = subjectcode,
                Description = description,
                UpdatedBy = updateby,
                UpdatedAt = DateTime.Now
            };
            try
            {
                await _unitOfWork.SubjectRepository.UpdateAsync(item);
                var result = await _unitOfWork.CommitAsync();
                if (result > 0)
                {
                    categoryObj = await _unitOfWork.SubjectRepository.GetId(item.Id);
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

        public async Task<R_Data> PutAsync(string name, string subjectcode, string description)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<Subject>(new Subject());
            var idMax = await _unitOfWork.SubjectRepository.Max();
            if (idMax.code != 1)
                throw new Exception();

            Subject item = new Subject()
            {
                Id = idMax.data + 1,
                Name = name,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Timer = DateTime.Now,
                Status = 1,
                SubjectCode = subjectcode,
                Description = description

            };

            try
            {
                _unitOfWork.SubjectRepository.Add(item);
                var result = await _unitOfWork.CommitAsync();
                if (result > 0)
                {
                    categoryObj = await _unitOfWork.SubjectRepository.GetId(item.Id);
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
