using API.Internship.Domain.Models;
using API.Internship.Domain.Services;
using API.Internship.OPS.Helper;
using API.Internship.ResData;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Internship.OPS.Controllers
{
    [Route("[controller]/[Action]"), ApiController]
    public class ScoreTypeController : ControllerBase
    {
        private readonly ILogger<ScoreTypeController> _logger;
        private readonly IScoreTypeService _ScoreTypeService;
        private readonly IScoreTypeHelper _ScoreTypeHelper;

        public ScoreTypeController(ILogger<ScoreTypeController> logger, IScoreTypeService ScoreTypeService, IScoreTypeHelper ScoreTypeHelper)
        {
            _logger = logger;
            _ScoreTypeService = ScoreTypeService;
            _ScoreTypeHelper = ScoreTypeHelper;
        }

        [HttpGet]
        public async Task<ActionResult<R_Data>> GetScoreTypeById(int id)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _ScoreTypeService.GetAsync(id);
                res = await _ScoreTypeHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>> GetListScoreType()
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                Expression<Func<ScoreType, bool>> filter;
                filter = w => w.Status == 1;
                res = await _ScoreTypeService.GetListAsync(filter);
                res = await _ScoreTypeHelper.MergeDataList(res);
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
                res = await _ScoreTypeService.Delete(id, updatedBy);
                res = await _ScoreTypeHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>> Update(ScoreType item)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _ScoreTypeService.PutAsync(item.Id, item.Name, item.Remark, item.Timer);
                res = await _ScoreTypeHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>> Create(ScoreType item)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _ScoreTypeService.PutAsync(item.Name,item.Remark);
                res = await _ScoreTypeHelper.MergeData(res);
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
