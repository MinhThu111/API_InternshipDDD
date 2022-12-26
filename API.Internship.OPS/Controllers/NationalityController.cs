using API.Internship.Domain.Models;
using API.Internship.Domain.Services;
using API.Internship.OPS.Helper;
using API.Internship.ResData;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Internship.OPS.Controllers
{
    [Route("[controller]/[Action]"), ApiController]
    public class NationalityController : ControllerBase
    {
        private readonly ILogger<NationalityController> _logger;
        private readonly INationalityService _nationalityService;
        private readonly INationalityHelper _nationalityHelper;

        public NationalityController(ILogger<NationalityController> logger, INationalityService nationalityService, INationalityHelper nationalityHelper)
        {
            _logger = logger;
            _nationalityService = nationalityService;
            _nationalityHelper = nationalityHelper;
        }

        [HttpGet]
        public async Task<ActionResult<R_Data>> GetListNationality()
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                Expression<Func<Nationality, bool>> filter;
                filter = w => w.Status == 1;
                res = await _nationalityService.GetListAsync(filter);
                res = await _nationalityHelper.MergeDataList(res);
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
        public async Task<ActionResult<R_Data>> GetNationalityById(int id)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _nationalityService.GetAsync(id);
                res = await _nationalityHelper.MergeData(res);
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
                res = await _nationalityService.Delete(id, updatedBy);
                res = await _nationalityHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>> Create(Nationality item)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _nationalityService.PutAsync(item.Name, item.NameSlug, item.Description);
                res = await _nationalityHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>> Update(Nationality item)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _nationalityService.PutAsync(item.Id, item.Name, item.NameSlug, item.Description, item.UpdatedBy, item.Timer);
                res = await _nationalityHelper.MergeData(res);
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
