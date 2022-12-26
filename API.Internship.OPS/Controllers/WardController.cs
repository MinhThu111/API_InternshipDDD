using API.Internship.Domain.Models;
using API.Internship.Domain.Services;
using API.Internship.OPS.Helper;
using API.Internship.ResData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Internship.OPS.Controllers
{
    [Route("[controller]/[Action]"), ApiController]
    public class WardController : ControllerBase
    {
        private readonly ILogger<WardController> _logger;
        private readonly IWardService _wardService;
        private readonly IWardHelper _wardHelper;

        public WardController(ILogger<WardController> logger, IWardService wardService, IWardHelper wardHelper)
        {
            _logger = logger;
            _wardService = wardService;
            _wardHelper = wardHelper;
        }

        [HttpGet]
        public async Task<ActionResult<R_Data>> getWardById(int id)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _wardService.GetAsync(id);
                res = await _wardHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>> getListWardByStatusDistrictId(int? districtId)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                Expression<Func<Ward, bool>> filter;
                filter = w => w.Status == 1 && w.DistrictId==districtId;
                filter.Compile();
                res = await _wardService.GetListAsync(filter);
                res = await _wardHelper.MergeDataList(res);
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
        public async Task<ActionResult<R_Data>> getListWard()
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                Expression<Func<Ward, bool>> filter;
                filter = w => w.Status == 1;
                res = await _wardService.GetListAsync(filter);
                res = await _wardHelper.MergeDataList(res);
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
                res = await _wardService.Delete(id, updatedBy);
                res = await _wardHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>> Update(Ward item)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _wardService.PutAsync(item.Id, item.Name, item.NameSlug, item.WardCode, item.UpdatedBy, item.Timer);
                res = await _wardHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>> Create(Ward item)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _wardService.PutAsync(item.Name, item.NameSlug, item.WardCode, item.DistrictId);
                res = await _wardHelper.MergeData(res);
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
