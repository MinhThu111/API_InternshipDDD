using API.Internship.Domain.Models;
using API.Internship.Domain.Services;
using API.Internship.OPS.Helper;
using API.Internship.ResData;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Internship.OPS.Controllers
{
    [Route("[controller]/[Action]"), ApiController]
    public class TeacherSubjectController : ControllerBase
    {
        private readonly ILogger<TeacherSubjectController> _logger;
        private readonly ITeacherSubjectService _TeacherSubjectService;
        private readonly ITeacherSubjectHelper _TeacherSubjectHelper;

        public TeacherSubjectController(ILogger<TeacherSubjectController> logger, ITeacherSubjectService TeacherSubjectService, ITeacherSubjectHelper TeacherSubjectHelper)
        {
            _logger = logger;
            _TeacherSubjectService = TeacherSubjectService;
            _TeacherSubjectHelper = TeacherSubjectHelper;
        }

        [HttpGet]
        public async Task<ActionResult<R_Data>> getTeacherSubjectById(int id)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _TeacherSubjectService.GetAsync(id);
                res = await _TeacherSubjectHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>> getListTeacherSubject()
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                Expression<Func<TeacherSubject, bool>> filter;
                filter = w => w.Status == 1;
                res = await _TeacherSubjectService.GetListAsync(filter);
                res = await _TeacherSubjectHelper.MergeDataList(res);
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
        public async Task<ActionResult<R_Data>> getListTeacherByIdSubject(int id)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                Expression<Func<TeacherSubject, bool>> filter;
                filter = w => w.Status == 1 && w.SubjectId==id;
                res = await _TeacherSubjectService.GetListAsync(filter);
                res = await _TeacherSubjectHelper.MergeDataList(res);
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
                res = await _TeacherSubjectService.Delete(id, updatedBy);
                res = await _TeacherSubjectHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>> Update(TeacherSubject item)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _TeacherSubjectService.PutAsync(item.Id, item.TeacherId, item.SubjectId, item.UpdatedBy, item.Timer);
                res = await _TeacherSubjectHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>> Create(TeacherSubject item)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _TeacherSubjectService.PutAsync(item.TeacherId, item.SubjectId, item.Remark);
                res = await _TeacherSubjectHelper.MergeData(res);
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
