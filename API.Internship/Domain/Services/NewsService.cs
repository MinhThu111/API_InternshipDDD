using API.Internship.Domain.Interfaces;
using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Linq;
using System.Linq.Expressions;
namespace API.Internship.Domain.Services
{
    public interface INewsService
    {
        Task<R_Data> GetAsync(int id);
        Task<R_Data> GetListAsync(Expression<Func<News, bool>> expression);
        Task<R_Data> GetListAsync();
        Task<R_Data> Delete(int id, int? updatedBy);
        Task<R_Data> PutAsync(int id,int? newscategoryid, string title,string titleslug,string description,string detail , DateTime? timer, int? updateby, string avatarurl);
        Task<R_Data> PutAsync(int? newscategoryid, string title, string description, string detail, DateTime? timer, int? createby, string avatarurl);
        Task<R_Data> PutAsync(int id, int? status, int? updatedBy, DateTime timer);
    }
    public class NewsService: INewsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<NewsService> _logger;
        public NewsService(IUnitOfWork unitOfWork, ILogger<NewsService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<R_Data> GetAsync(int id)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<News>(new News());
            try
            {
                categoryObj = await _unitOfWork.NewsRepository.GetId(id);
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
        public async Task<R_Data> GetListAsync(Expression<Func<News, bool>> expression)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var lstObj = await Task.FromResult<List<News>>(new List<News>());
            try
            {
                lstObj = (await _unitOfWork.NewsRepository.ListAsync(expression)).ToList();
                lstObj = lstObj.OrderByDescending(x => x.Id).ToList();
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
            R_Data res = new R_Data() { result = 1, data = null, error = errObj };
            var lstNews = await Task.FromResult<List<News>>(new List<News>());
            try
            {
                Expression<Func<News, bool>> filter;
                filter = w => w.Status == 1;
                filter.Compile();
                lstNews = (await _unitOfWork.NewsRepository.ListAsync(filter)).ToList();

                var datacount = from item in lstNews
                                group item by item.NewsCategoryId into lstitem
                                select new
                                {
                                    name = lstitem.Key,
                                    count = lstitem.Count(),
                                };

                if (datacount.Count() <= 0)
                    errObj.message = "Load data is successful and do not data to show!";
                else
                    res.data = datacount;
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
            var categoryObj = await Task.FromResult<News>(new News());
            try
            {
                Expression<Func<News, bool>> filter;
                filter = w => w.Id == id;
                categoryObj = _unitOfWork.NewsRepository.Find(filter);
                if (categoryObj == null)
                {
                    res.result = 0;
                    errObj.message = $"Không tìm thấy {id} để xóa.";
                }
                else
                {
                    _unitOfWork.NewsRepository.Delete(categoryObj);
                    var result = await _unitOfWork.CommitAsync();
                    if (result > 0)
                    {
                        categoryObj = await _unitOfWork.NewsRepository.GetId(id);
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
        public async Task<R_Data> PutAsync(int id,int? newscategoryid, string title,string titleslug,string description,string detail, DateTime? timer, int? updateby, string avatarurl)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<News>(new News());
            var existNews = await _unitOfWork.NewsRepository.GetId(id);
            if (existNews == null)
            {
                throw new Exception($"Grade {id} không tìm thấy.");
            }
            if (existNews.Timer > timer)
            {
                res.result = 0;
                res.data = null;
                res.error = new error() { code = 201, message = "Thông tin đã được cập nhật lại trước đó. Vui lòng hủy thao tác và thực hiện lại để dữ liệu đồng bộ!" };
                return res;
            }
            News item = new News()
            {
                Id = existNews.Id,
                NewsCategoryId = newscategoryid,
                Title = title,
                TitleSlug = titleslug,
                Description = description,
                Detail = detail,
                Status = existNews.Status,
                AvatarUrl= avatarurl,
                UpdatedBy = updateby,
                UpdatedAt = DateTime.Now,
                CreatedBy = existNews.CreatedBy,
                CreatedAt = existNews.CreatedAt,
                Timer = DateTime.Now
            };
            try
            {
                await _unitOfWork.NewsRepository.UpdateAsync(item);
                var result = await _unitOfWork.CommitAsync();
                if (result > 0)
                {
                    categoryObj = await _unitOfWork.NewsRepository.GetId(item.Id);
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
        public async Task<R_Data> PutAsync(int? newscategoryid, string title, string description, string detail, DateTime? timer, int? createby, string avatarurl)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<News>(new News());
            var idMax = await _unitOfWork.NewsRepository.Max();
            if (idMax.code != 1)
                throw new Exception();

            News item = new News()
            {
                Id = idMax.data + 1,
                NewsCategoryId = newscategoryid,
                Title = title,
                TitleSlug = null,
                Description = description,
                Detail = detail,
                AvatarUrl= avatarurl,
                Status = 1,
                UpdatedBy = createby,
                UpdatedAt = DateTime.Now,
                CreatedAt= DateTime.Now,
                CreatedBy= createby,
                Timer =DateTime.Now

            };

            try
            {
                _unitOfWork.NewsRepository.Add(item);
                var result = await _unitOfWork.CommitAsync();
                if (result > 0)
                {
                    categoryObj = await _unitOfWork.NewsRepository.GetId(item.Id);
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
            var categoryObj = await Task.FromResult<News>(new News());


            var existingNews = await _unitOfWork.NewsRepository.GetId(id);
            //var existingNews = new InternshipContext().Newss.FirstOrDefault(f => f.Id == id);
            if (existingNews == null)
                throw new Exception($"News {id} không tìm thấy.");

            if (existingNews.Timer > timer)
            {
                res.result = 0;
                res.data = null;
                res.error = new error() { code = 201, message = "Thông tin đã được cập nhật lại trước đó. Vui lòng hủy thao tác và thực hiện lại để dữ liệu đồng bộ!" };
                return res;
            }

            News item = new News
            {
                Id = existingNews.Id,
                NewsCategoryId=existingNews.NewsCategoryId,
                Title=existingNews.Title,
                TitleSlug=existingNews.TitleSlug,
                Description=existingNews.Description,
                Detail=existingNews.Detail,
                AvatarUrl=existingNews.AvatarUrl,
                CreatedAt = existingNews.CreatedAt,
                CreatedBy = existingNews.CreatedBy,
                UpdatedAt = DateTime.Now,
                UpdatedBy = updatedBy,
                Status = status,
                Timer = DateTime.Now,
            };

            try
            {
                await _unitOfWork.NewsRepository.UpdateAsync(item);
                //await _unitOfWork.NewsRepository.UpdateAsync(item);
                var result = await _unitOfWork.CommitAsync();
                if (result > 0)
                {
                    categoryObj = await _unitOfWork.NewsRepository.GetId(item.Id);
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
