using API.Internship.Domain.Models;
using API.Internship.Domain.Services;
using API.Internship.OPS.Helper;
using API.Internship.ResData;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
namespace API.Internship.OPS.Controllers
{
    [Route("[controller]/[Action]"), ApiController]
    public class GradeStudentController : ControllerBase
    {
        private readonly ILogger<GradeStudentController> _logger;
        private readonly IGradeStudentService _gradestudentService;
        private readonly IGradeStudentHelper _gradestudentHelp;

        public GradeStudentController(ILogger<GradeStudentController> logger, IGradeStudentService gradestudentService, IGradeStudentHelper gradestudentHelp)
        {
            _logger = logger;
            _gradestudentService = gradestudentService;
            _gradestudentHelp = gradestudentHelp;
        }

        [HttpGet]
        public async Task<ActionResult<R_Data>> GetGradeStudentById(int id)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _gradestudentService.GetAsync(id);
                res = await _gradestudentHelp.MergeData(res);
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
        public async Task<ActionResult<R_Data>> GetListGradeStudent()
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                Expression<Func<GradeStudent, bool>> filter;
                filter = w => w.Status == 1;
                res = await _gradestudentService.GetListAsync(filter);
                res = await _gradestudentHelp.MergeDataList(res);
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
                res = await _gradestudentService.Delete(id, updatedBy);
                res = await _gradestudentHelp.MergeData(res);
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
        public async Task<ActionResult<R_Data>> Update(GradeStudent item)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _gradestudentService.PutAsync(item.Id, item.GradeId, item.StudentId, item.PositionId, item.Timer, item.UpdatedBy);
                res = await _gradestudentHelp.MergeData(res);
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
        public async Task<ActionResult<R_Data>> Create(GradeStudent item)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _gradestudentService.PutAsync(item.GradeId, item.StudentId, item.PositionId);
                res = await _gradestudentHelp.MergeData(res);
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
