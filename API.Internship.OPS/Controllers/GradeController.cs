
using API.Internship.Domain.Models;
using API.Internship.Domain.Services;
using API.Internship.OPS.Helper;
using API.Internship.ResData;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Internship.OPS.Controllers;

[Route("[controller]/[Action]"), ApiController]
public class GradeController : ControllerBase
{
    private readonly ILogger<GradeController> _logger;
    private readonly IGradeService _gradeService;
    private readonly IGradeHelper _gradeHelper;

    public GradeController(ILogger<GradeController> logger, IGradeService gradeService, IGradeHelper gradeHelper)
    {
        _logger = logger;
        _gradeService = gradeService;
        _gradeHelper = gradeHelper;
    }

    [HttpGet]
    public async Task<ActionResult<R_Data>> getGradeById(int id)
    {
        R_Data res = new R_Data { result = 1, data = null, error = new error() };
        try
        {
            res = await _gradeService.GetAsync(id);
            res = await _gradeHelper.MergeData(res);
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
    public async Task<ActionResult<R_Data>> getListGrade()
    {
        R_Data res = new R_Data { result = 1, data = null, error = new error() };
        try
        {
            Expression<Func<Grade, bool>> filter;
            filter = w => w.Status == 1;
            filter.Compile();
            res = await _gradeService.GetListAsync(filter);
            res = await _gradeHelper.MergeDataList(res);
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
    public async Task<ActionResult<R_Data>> getListGradeByStatus(int? status)
    {
        R_Data res = new R_Data { result = 1, data = null, error = new error() };
        try
        {
            Expression<Func<Grade, bool>> filter;
            filter = w => w.Status == status;
            filter.Compile();
            res = await _gradeService.GetListAsync(filter);
            res = await _gradeHelper.MergeDataList(res);
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
    public async Task<ActionResult<R_Data>> getListGradeBySequenceStatus(string sequenceStatus)
    {
        R_Data res = new R_Data { result = 1, data = null, error = new error() };
        try
        {
            List<int?> lstStatus = new List<int?>();
            if (string.IsNullOrEmpty(sequenceStatus))
                return new R_Data() { result = 0, data = null, error = new error() { code = 201, message = "Dãy trạng thái chưa nhập giá trị. Dãy trạng thái là ký số và cách nhau bởi dấu phẩy [,]" } };
            try
            {
                foreach (string s in sequenceStatus.Split(","))
                    if (!string.IsNullOrEmpty(s))
                        lstStatus.Add(Convert.ToInt32(s.Replace(".", "").Replace(" ", "")));
            }
            catch (Exception) { }
            Expression<Func<Grade, bool>> filter;
            filter = w => lstStatus.Contains(w.Status);
            filter.Compile();
            res = await _gradeService.GetListAsync(filter);
            res = await _gradeHelper.MergeDataList(res);
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
    public async Task<ActionResult<R_Data>> Create(Grade ori)
    {
        R_Data res = new R_Data { result = 1, data = null, error = new error() };
        try
        {
            res = await _gradeService.PutAsync(ori.Name, ori.GradeCode, ori.TeacherId ?? 0, ori.Remark, ori.CreatedBy ?? 0);
            res = await _gradeHelper.MergeData(res);
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
    public async Task<ActionResult<R_Data>> Update(Grade ori)
    {
        R_Data res = new R_Data { result = 1, data = null, error = new error() };
        try
        {
            res = await _gradeService.PutAsync(ori.Id, ori.Name, ori.GradeCode, ori.TeacherId ?? 0, ori.Remark, ori.Status, ori.UpdatedBy ?? 0, ori.Timer);
            res = await _gradeHelper.MergeData(res);
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
    public async Task<ActionResult<R_Data>> UpdateStatus(Grade ori)
    {
        R_Data res = new R_Data { result = 1, data = null, error = new error() };
        try
        {
            res = await _gradeService.PutAsync(ori.Id, ori.Status ?? 0, ori.UpdatedBy ?? 0, ori.Timer);
            res = await _gradeHelper.MergeData(res);
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
            res = await _gradeService.Delete(id, updatedBy);
            res = await _gradeHelper.MergeData(res);
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
