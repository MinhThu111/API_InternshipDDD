using API.Internship.Domain.Models;
using API.Internship.Domain.Services;
using API.Internship.OPS.Helper;
using API.Internship.ResData;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;


namespace API.Internship.OPS.Controllers
{
    [Route("[controller]/[Action]"), ApiController]
    public class SchoolInfoController : ControllerBase
    {
        private readonly ILogger<SchoolInfoController> _logger;
        private readonly ISchoolInfoService _schoolInfoService;
        private readonly ISchoolInfoHelper _schoolInfoHelper;

        public SchoolInfoController(ILogger<SchoolInfoController> logger, ISchoolInfoService schoolInfoService, ISchoolInfoHelper schoolInfoHelper)
        {
            _logger = logger;
            _schoolInfoService = schoolInfoService;
            _schoolInfoHelper = schoolInfoHelper;
        }

        [HttpGet]
        public async Task<ActionResult<R_Data>> GetSchoolInfoById(int id)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _schoolInfoService.GetAsync(id);
                res = await _schoolInfoHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>> GetListSchoolInfo()
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                Expression<Func<SchoolInfo, bool>> filter;
                filter = w => w.Status == 1;
                res = await _schoolInfoService.GetListAsync(filter);
                res = await _schoolInfoHelper.MergeDataList(res);
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
                res = await _schoolInfoService.Delete(id, updatedBy);
                res = await _schoolInfoHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>> Update(SchoolInfo item)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _schoolInfoService.PutAsync(item.Id, item.Name, item.Timer, item.NameEn, item.Sologan, item.UpdatedBy, item.EstablishDate);
                res = await _schoolInfoHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>> Create(SchoolInfo item)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _schoolInfoService.PutAsync(item.Name, item.NameEn, item.AddressId, item.Sologan, item.EstablishDate);
                res = await _schoolInfoHelper.MergeData(res);
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
