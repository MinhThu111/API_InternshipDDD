using API.Internship.Domain.Models;
using API.Internship.Domain.Services;
using API.Internship.OPS.Helper;
using API.Internship.ResData;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;


namespace API.Internship.OPS.Controllers
{
    [Route("[controller]/[Action]"), ApiController]
    public class PositionController : ControllerBase
    {
        private readonly ILogger<PositionController> _logger;
        private readonly IPositionService _positionService;
        private readonly IPositionHelper _positionHelper;

        public PositionController(ILogger<PositionController> logger, IPositionService positionService, IPositionHelper positionHelper)
        {
            _logger = logger;
            _positionService = positionService;
            _positionHelper = positionHelper;
        }

        [HttpGet]
        public async Task<ActionResult<R_Data>> GetPositionById(int id)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _positionService.GetAsync(id);
                res = await _positionHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>> GetListPosition()
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                Expression<Func<Position, bool>> filter;
                filter = w => w.Status == 1;
                res = await _positionService.GetListAsync(filter);
                res = await _positionHelper.MergeDataList(res);
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
                res = await _positionService.Delete(id, updatedBy);
                res = await _positionHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>> Update(Position item)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _positionService.PutAsync(item.Id, item.Name, item.Timer, item.PositionCode, item.UpdatedBy);
                res = await _positionHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>> Create(Position item)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _positionService.PutAsync(item.Name, item.PositionCode, item.Remark);
                res = await _positionHelper.MergeData(res);
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
