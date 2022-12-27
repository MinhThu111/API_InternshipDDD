using API.Internship.Domain.Models;
using API.Internship.Domain.Services;
using API.Internship.OPS.Helper;
using API.Internship.ResData;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Internship.OPS.Controllers
{
    [Route("[controller]/[Action]"), ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ILogger<SubjectController> _logger;
        private readonly ISubjectService _SubjectService;
        private readonly ISubjectHelper _SubjectHelper;

        public SubjectController(ILogger<SubjectController> logger, ISubjectService SubjectService, ISubjectHelper SubjectHelper)
        {
            _logger = logger;
            _SubjectService = SubjectService;
            _SubjectHelper = SubjectHelper;
        }

        [HttpGet]
        public async Task<ActionResult<R_Data>> getSubjectById(int id)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _SubjectService.GetAsync(id);
                res = await _SubjectHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>> getListSubject()
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                Expression<Func<Subject, bool>> filter;
                filter = w => w.Status == 1;
                res = await _SubjectService.GetListAsync(filter);
                res = await _SubjectHelper.MergeDataList(res);
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
                res = await _SubjectService.Delete(id, updatedBy);
                res = await _SubjectHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>> Update(Subject item)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _SubjectService.PutAsync(item.Id, item.Name, item.SubjectCode, item.Description, item.UpdatedBy, item.Timer);
                res = await _SubjectHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>> Create(Subject item)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _SubjectService.PutAsync(item.Name, item.SubjectCode, item.Description);
                res = await _SubjectHelper.MergeData(res);
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
