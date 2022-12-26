using API.Internship.Domain.Models;
using API.Internship.Domain.Services;
using API.Internship.OPS.Helper;
using API.Internship.ResData;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Internship.OPS.Controllers
{
    [Route("[controller]/[Action]"), ApiController]
    public class ReligionController : ControllerBase
    {
        private readonly ILogger<ReligionController> _logger;
        private readonly IReligionService _religionService;
        private readonly IReligionHelper _religionHelper;

        public ReligionController(ILogger<ReligionController> logger, IReligionService religionService, IReligionHelper religionHelper)
        {
            _logger = logger;
            _religionService = religionService;
            _religionHelper = religionHelper;
        }

        [HttpGet]
        public async Task<ActionResult<R_Data>> GetReligionById(int id)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _religionService.GetAsync(id);
                res = await _religionHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>> GetListReligion()
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                Expression<Func<Religion, bool>> filter;
                filter = w => w.Status == 1;
                res = await _religionService.GetListAsync(filter);
                res = await _religionHelper.MergeDataList(res);
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
                res = await _religionService.Delete(id, updatedBy);
                res = await _religionHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>> Create (Religion item)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _religionService.PutAsync(item.Name, item.NameSlug,item.Description);
                res = await _religionHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>> Update(Religion item)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _religionService.PutAsync(item.Id, item.Name, item.NameSlug, item.Description, item.UpdatedBy, item.Timer);
                res = await _religionHelper.MergeData(res);
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
