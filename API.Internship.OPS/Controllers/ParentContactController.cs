using API.Internship.Domain.Models;
using API.Internship.Domain.Services;
using API.Internship.OPS.Helper;
using API.Internship.ResData;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Internship.OPS.Controllers
{
    [Route("[controller]/[Action]"), ApiController]
    public class ParentContactController : ControllerBase
    {
        private readonly ILogger<ParentContactController> _logger;
        private readonly IParentContactService _parentContactService;
        private readonly IParentContactHelper _parentContactHelper;

        public ParentContactController(ILogger<ParentContactController> logger, IParentContactService parentContactService, IParentContactHelper parentContactHelper)
        {
            _logger = logger;
            _parentContactService = parentContactService;
            _parentContactHelper = parentContactHelper;
        }

        [HttpGet]
        public async Task<ActionResult<R_Data>> GetParentContactById(int id)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _parentContactService.GetAsync(id);
                res = await _parentContactHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>> GetListParentContact()
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                Expression<Func<ParentContact, bool>> filter;
                filter = w => w.Status == 1;
                res = await _parentContactService.GetListAsync(filter);
                res = await _parentContactHelper.MergeDataList(res);
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
                res = await _parentContactService.Delete(id, updatedBy);
                res = await _parentContactHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>> Update(ParentContact item)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _parentContactService.PutAsync(item.Id, item.FirstName, item.LastName, item.Timer, item.AddressId, item.PhoneNumber);
                res = await _parentContactHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>> Create(ParentContact item)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _parentContactService.PutAsync(item.FirstName, item.LastName, item.AddressId, item.PhoneNumber, item.Email);
                res = await _parentContactHelper.MergeData(res);
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
