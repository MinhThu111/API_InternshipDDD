using API.Internship.Domain.Models;
using API.Internship.Domain.Services;
using API.Internship.OPS.Helper;
using API.Internship.ResData;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Internship.OPS.Controllers
{
    [Route("[controller]/[Action]"), ApiController]
    public class NewsController : ControllerBase
    {
        private readonly ILogger<NewsController> _logger;
        private readonly INewsService _newsService;
        private readonly INewsHelper _newsHelper;

        public NewsController(ILogger<NewsController> logger, INewsService newsService, INewsHelper newsHelper)
        {
            _logger = logger;
            _newsService = newsService;
            _newsHelper = newsHelper;
        }

        [HttpGet]
        public async Task<ActionResult<R_Data>> getNewsById(int id)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _newsService.GetAsync(id);
                res = await _newsHelper.MergeData(res);
            }
            catch (Exception ex)
            {
                res.result = 0;
                res.data = null;
                res.error = new error { code = -1, message = ex.Message };
            }
            return res;
        }
        [HttpGet]
        public async Task<ActionResult<R_Data>> getListNewsBySequenceStatus(string sequenceStatus, string lstcategoryid)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                List<int?> lstStatus = new List<int?>();
                List<int?> lstnewscategory = new List<int?>();
                if (string.IsNullOrEmpty(sequenceStatus))
                    return new R_Data() { result = 0, data = null, error = new error() { code = 201, message = "Dãy trạng thái chưa nhập giá trị. Dãy trạng thái là ký số và cách nhau bởi dấu phẩy [,]" } };
                try
                {
                    foreach (string s in sequenceStatus.Split(","))
                        if (!string.IsNullOrEmpty(s))
                            lstStatus.Add(Convert.ToInt32(s.Replace(".", "").Replace(" ", "")));
                    foreach (string s in lstcategoryid.Split(","))
                        if (!string.IsNullOrEmpty(s))
                            lstnewscategory.Add(Convert.ToInt32(s.Replace(".", "").Replace(" ", "")));
                }
                catch (Exception) { }
                Expression<Func<News, bool>> filter;
                if (lstnewscategory[0] == 0)
                {
                   
                    filter = w => lstStatus.Contains(w.Status);
                    filter.Compile();
                }
                else
                {
                    filter = w => lstStatus.Contains(w.Status) && lstnewscategory.Contains(w.NewsCategoryId);
                    filter.Compile();
                }
               
                res = await _newsService.GetListAsync(filter);
                res = await _newsHelper.MergeDataList(res);
                //res = await _personHelper.MergeDynamicList(res);
            }
            catch (Exception ex)
            {
                res.result = 0;
                res.data = null;
                res.error = new error { code = -1, message = ex.Message };
            }
            return res;
        }
        [HttpGet]
        public async Task<ActionResult<R_Data>> getListNewsByCategoryId(int newcategoryid)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                Expression<Func<News, bool>> filter;
                filter = w => w.Status == 1 && w.NewsCategoryId==newcategoryid;
                filter.Compile();
                res = await _newsService.GetListAsync(filter);
                res = await _newsHelper.MergeDataList(res);
            }
            catch (Exception ex)
            {
                res.result = 0;
                res.data = null;
                res.error = new error { code = -1, message = ex.Message };
            }
            return res;
        }
        [HttpGet]
        public async Task<ActionResult<R_Data>> getListNews()
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                Expression<Func<News, bool>> filter;
                filter = w => w.Status == 1;
                filter.Compile();
                res = await _newsService.GetListAsync(filter);
                res = await _newsHelper.MergeDataList(res);
            }
            catch (Exception ex)
            {
                res.result = 0;
                res.data = null;
                res.error = new error { code = -1, message = ex.Message };
            }
            return res;
        }
        [HttpGet]
        public async Task<ActionResult<R_Data>> getListCountNewsByCategoryId()
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                Expression<Func<News, bool>> filter;
                filter = w => w.Status == 1;
                filter.Compile();
                res = await _newsService.GetListAsync();
                res = await _newsHelper.MergeDataList(res);
            }
            catch (Exception ex)
            {
                res.result = 0;
                res.data = null;
                res.error = new error { code = -1, message = ex.Message };
            }
            return res;
        }
        [HttpDelete]
        public async Task<ActionResult<R_Data>> Delete(int id, int? updatedBy)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _newsService.Delete(id, updatedBy);
                res = await _newsHelper.MergeData(res);
            }
            catch (Exception ex)
            {
                res.result = 0;
                res.data = null;
                res.error = new error { code = -1, message = ex.Message };
            }
            return res;
        }
        [HttpPut]
        public async Task<ActionResult<R_Data>> Update(News item)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _newsService.PutAsync(item.Id, item.NewsCategoryId, item.Title, item.TitleSlug, item.Description, item.Detail, item.Timer, item.UpdatedBy, item.AvatarUrl);
                res = await _newsHelper.MergeData(res);
            }
            catch (Exception ex)
            {
                res.result = 0;
                res.data = null;
                res.error = new error { code = -1, message = ex.Message };
            }
            return res;
        }
        [HttpPut]
        public async Task<ActionResult<R_Data>> UpdateStatus(News ori)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _newsService.PutAsync(ori.Id, ori.Status ?? 0, ori.UpdatedBy ?? 0, ori.Timer);
                res = await _newsHelper.MergeData(res);
            }
            catch (Exception ex)
            {
                res.result = 0;
                res.data = null;
                res.error = new error { code = -1, message = ex.Message };
            }
            return res;
        }
        [HttpPost]
        public async Task<ActionResult<R_Data>> Create(News item)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _newsService.PutAsync(item.NewsCategoryId, item.Title, item.Description, item.Detail, item.Timer, item.CreatedBy, item.AvatarUrl);
                res = await _newsHelper.MergeData(res);
            }
            catch (Exception ex)
            {
                res.result = 0;
                res.data = null;
                res.error = new error { code = -1, message = ex.Message };
            }
            return res;
        }
    }
}
