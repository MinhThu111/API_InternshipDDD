using API.Internship.Domain.Models;
using API.Internship.Domain.Services;
using API.Internship.OPS.Helper;
using API.Internship.ResData;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Internship.OPS.Controllers
{
    [Route("[controller]/[Action]"), ApiController]
    public class NewsCategoryController : ControllerBase
    {
        private readonly ILogger<NewsCategoryController> _logger;
        private readonly INewsCategoryService _newsCategoryService;
        private readonly INewsCategoryHelper _newsCategoryHelper;

        public NewsCategoryController(ILogger<NewsCategoryController> logger, INewsCategoryService newsCategoryService, INewsCategoryHelper newsCategoryHelper)
        {
            _logger = logger;
            _newsCategoryService = newsCategoryService;
            _newsCategoryHelper = newsCategoryHelper;
        }

        [HttpGet]
        public async Task<ActionResult<R_Data>> getNewsCategoryById(int id)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _newsCategoryService.GetAsync(id);
                res = await _newsCategoryHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>> getListNewsCategory()
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                Expression<Func<NewsCategory, bool>> filter;
                filter = w => w.Status == 1;
                filter.Compile();
                res = await _newsCategoryService.GetListAsync(filter);
                res = await _newsCategoryHelper.MergeDataList(res);
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
        public async Task<ActionResult<R_Data>> getNewCategoryMenu()
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _newsCategoryService.GetListAsync();
                res = await _newsCategoryHelper.MergeDynamicList(res);
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
                res = await _newsCategoryService.Delete(id, updatedBy);
                res = await _newsCategoryHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>> Update(NewsCategory item)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _newsCategoryService.PutAsync(item.Id, item.ParentId, item.Name, item.Type, item.Timer, item.UpdatedBy);
                res = await _newsCategoryHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>> Create(NewsCategory item)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _newsCategoryService.PutAsync(item.ParentId, item.Name, item.Type, item.Timer, item.UpdatedBy);
                res = await _newsCategoryHelper.MergeData(res);
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
