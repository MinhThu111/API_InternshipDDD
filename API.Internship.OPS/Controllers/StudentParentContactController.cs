using API.Internship.Domain.Models;
using API.Internship.Domain.Services;
using API.Internship.OPS.Helper;
using API.Internship.ResData;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
namespace API.Internship.OPS.Controllers
{
    [Route("[controller]/[Action]"), ApiController]
    public class StudentParentContactController : ControllerBase
    {
        private readonly ILogger<StudentParentContactController> _logger;
        private readonly IStudentParentContactService _StudentParentContactService;
        private readonly IStudentParentContactHelper _StudentParentContactHelper;

        public StudentParentContactController(ILogger<StudentParentContactController> logger, IStudentParentContactService StudentParentContactService, IStudentParentContactHelper StudentParentContactHelper)
        {
            _logger = logger;
            _StudentParentContactService = StudentParentContactService;
            _StudentParentContactHelper = StudentParentContactHelper;
        }

        [HttpGet]
        public async Task<ActionResult<R_Data>> GetStudentParentContactById(int id)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _StudentParentContactService.GetAsync(id);
                res = await _StudentParentContactHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>> GetListStudentParentContact()
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                Expression<Func<StudentParentContact, bool>> filter;
                filter = w => w.Status == 1;
                res = await _StudentParentContactService.GetListAsync(filter);
                res = await _StudentParentContactHelper.MergeDataList(res);
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
                res = await _StudentParentContactService.Delete(id, updatedBy);
                res = await _StudentParentContactHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>> Update(StudentParentContact item)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _StudentParentContactService.PutAsync(item.Id, item.StudentId, item.ParentContactId, item.Timer);
                res = await _StudentParentContactHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>> Create(StudentParentContact item)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _StudentParentContactService.PutAsync(item.StudentId, item.ParentContactId);
                res = await _StudentParentContactHelper.MergeData(res);
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
