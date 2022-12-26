using API.Internship.Domain.Interfaces;
using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Linq.Expressions;
namespace API.Internship.Domain.Services
{
    public interface INewsCategoryService
    {
        Task<R_Data> GetAsync(int id);
        Task<R_Data> GetListAsync(Expression<Func<NewsCategory, bool>> expression);
        Task<R_Data> GetListAsync();
        Task<R_Data> Delete(int id, int? updatedBy);
        Task<R_Data> PutAsync(int id, int? parentid,string name, int? type, DateTime? timer, int? updateby);
        Task<R_Data> PutAsync(int? parentid, string name, int? type, DateTime? timer, int? updateby);
    }
    public class NewsCategoryService: INewsCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<NewsCategoryService> _logger;
        public NewsCategoryService(IUnitOfWork unitOfWork, ILogger<NewsCategoryService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<R_Data> GetAsync(int id)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<NewsCategory>(new NewsCategory());
            try
            {
                categoryObj = await _unitOfWork.NewsCategoryRepository.GetId(id);
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
        public async Task<R_Data> GetListAsync(Expression<Func<NewsCategory, bool>> expression)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var lstObj = await Task.FromResult<List<NewsCategory>>(new List<NewsCategory>());
            try
            {
                lstObj = (await _unitOfWork.NewsCategoryRepository.ListAsync(expression)).ToList();
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
        public async Task<R_Data> GetListAsync()
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var lstObj = await Task.FromResult<List<NewsCategory>>(new List<NewsCategory>());
            try
            {
                Expression<Func<NewsCategory, bool>> filter;
                filter = w => w.Status == 1;
                filter.Compile();
                lstObj = (await _unitOfWork.NewsCategoryRepository.ListAsync(filter)).ToList();
               




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
            var categoryObj = await Task.FromResult<NewsCategory>(new NewsCategory());
            try
            {
                Expression<Func<NewsCategory, bool>> filter;
                filter = w => w.Id == id;
                categoryObj = _unitOfWork.NewsCategoryRepository.Find(filter);
                if (categoryObj == null)
                {
                    res.result = 0;
                    errObj.message = $"Không tìm thấy {id} để xóa.";
                }
                else
                {
                    _unitOfWork.NewsCategoryRepository.Delete(categoryObj);
                    var result = await _unitOfWork.CommitAsync();
                    if (result > 0)
                    {
                        categoryObj = await _unitOfWork.NewsCategoryRepository.GetId(id);
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
        public async Task<R_Data> PutAsync(int id, int? parentid, string name, int? type, DateTime? timer, int? updateby)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<NewsCategory>(new NewsCategory());
            var existNewsCategory = await _unitOfWork.NewsCategoryRepository.GetId(id);
            if (existNewsCategory == null)
            {
                throw new Exception($"Grade {id} không tìm thấy.");
            }
            if (existNewsCategory.Timer > timer)
            {
                res.result = 0;
                res.data = null;
                res.error = new error() { code = 201, message = "Thông tin đã được cập nhật lại trước đó. Vui lòng hủy thao tác và thực hiện lại để dữ liệu đồng bộ!" };
                return res;
            }
            NewsCategory item = new NewsCategory()
            {
                ParentId=parentid,
                Name=name,
                Type=type,
                UpdatedBy = updateby,
                UpdatedAt = DateTime.Now
            };
            try
            {
                await _unitOfWork.NewsCategoryRepository.UpdateAsync(item);
                var result = await _unitOfWork.CommitAsync();
                if (result > 0)
                {
                    categoryObj = await _unitOfWork.NewsCategoryRepository.GetId(item.Id);
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
        public async Task<R_Data> PutAsync(int? parentid, string name, int? type, DateTime? timer, int? updateby)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<NewsCategory>(new NewsCategory());
            var idMax = await _unitOfWork.NewsCategoryRepository.Max();
            if (idMax.code != 1)
                throw new Exception();

            NewsCategory item = new NewsCategory()
            {
                Id = idMax.data + 1,
                ParentId = parentid,
                Name = name,
                Type = type,
                UpdatedBy = updateby,
                UpdatedAt = DateTime.Now,
                Status = 1,
                Timer = DateTime.Now

            };

            try
            {
                _unitOfWork.NewsCategoryRepository.Add(item);
                var result = await _unitOfWork.CommitAsync();
                if (result > 0)
                {
                    categoryObj = await _unitOfWork.NewsCategoryRepository.GetId(item.Id);
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
