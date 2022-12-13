using API.Internship.Domain.Models;
using API.Internship.Domain.Services;
using API.Internship.OPS.Helper;
using API.Internship.ResData;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Internship.OPS.Controllers
{
    [Route("[controller]/[Action]"), ApiController]
    public class AddressController : ControllerBase
    {
        private readonly ILogger<AddressController> _logger;
        private readonly IAddressService _addressService;
        private readonly IAddressHelper _addressHelper;

        public AddressController(ILogger<AddressController> logger, IAddressService addressService, IAddressHelper addressHelper)
        {
            _logger = logger;
            _addressService = addressService;
            _addressHelper = addressHelper;
        }

        [HttpGet]
        public async Task<ActionResult<R_Data>> getListAddressBySequenceStatus(string sequenceStatus)
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
                Expression<Func<Address, bool>> filter;
                filter = w => lstStatus.Contains(w.Status);
                filter.Compile();
                res = await _addressService.GetListAsync(filter);
                res = await _addressHelper.MergeDataList(res);
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
        public async Task<ActionResult<R_Data>>getListAddress()
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                Expression<Func<Address, bool>> filter;
                filter = w => w.Status == 1;
                res = await _addressService.GetListAsync(filter);
                res = await _addressHelper.MergeDataList(res);
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
        public async Task<ActionResult<R_Data>>getAddressById(int id)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _addressService.GetAsync(id);
                res = await _addressHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>>Delete(int id, int? updatedBy)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _addressService.Delete(id, updatedBy);
                res = await _addressHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>>Create(Address item)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _addressService.PutAsync(item.Title, item.AddressNumber, item.AddressText, item.CountryId, item.ProvinceId, item.DistrictId, item.WardId);
                res = await _addressHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>>Update(Address item)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _addressService.PutAsync(item.Id, item.Title, item.AddressNumber, item.AddressText, item.CountryId, item.ProvinceId, item.DistrictId, item.WardId, item.Timer);
                res=await _addressHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>> UpdateStatus(Address ori)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _addressService.PutAsync(ori.Id, ori.Status ?? 0, ori.UpdatedBy ?? 0, ori.Timer);
                res = await _addressHelper.MergeData(res);
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
