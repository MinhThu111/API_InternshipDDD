using API.Internship.Domain.Models;
using API.Internship.Domain.Services;
using API.Internship.OPS.Helper;
using API.Internship.ResData;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Internship.OPS.Controllers
{
    [Route("[controller]/[Action]"), ApiController]
    public class FolkController : ControllerBase
    {
        private readonly ILogger<FolkController> _logger;
        private readonly IFolkService _folkService;
        private readonly IFolkHelper _folkHelper;

        public FolkController(ILogger<FolkController> logger, IFolkService folkService, IFolkHelper folkHelper)
        {
            _logger = logger;
            _folkService = folkService;
            _folkHelper = folkHelper;
        }

        [HttpGet]
        public async Task<ActionResult<R_Data>>GetListFolk()
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                Expression<Func<Folk, bool>> filter;
                filter = w => w.Status == 1;
                res = await _folkService.GetListAsync(filter);
                res = await _folkHelper.MergeDataList(res);
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
        public async Task<ActionResult<R_Data>>GetFolkById(int id)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _folkService.GetAsync(id);
                res = await _folkHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>>Delete(int id, int? updatedBy)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _folkService.Delete(id, updatedBy);
                res = await _folkHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>>Create(Folk item)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _folkService.PutAsync(item.Name, item.NameSlug, item.Description);
                res = await _folkHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>>Update(Folk item)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _folkService.PutAsync(item.Id, item.Name, item.NameSlug, item.Description, item.UpdatedBy, item.Timer);
                res=await _folkHelper.MergeData(res);
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
