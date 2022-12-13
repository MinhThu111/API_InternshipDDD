using API.Internship.Domain.Models;
using API.Internship.Domain.Services;
using API.Internship.OPS.Helper;
using API.Internship.ResData;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Internship.OPS.Controllers
{
    [Route("[controller]/[Action]"), ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ILogger<CountryController> _logger;
        private readonly ICountryService _countryService;
        private readonly ICountryHelper _countryHelper;

        public CountryController(ILogger<CountryController> logger, ICountryService countryService, ICountryHelper countryHelper)
        {
            _logger = logger;
            _countryService = countryService;
            _countryHelper = countryHelper;
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
                Expression<Func<Country, bool>> filter;
                filter = w => lstStatus.Contains(w.Status);
                filter.Compile();
                res = await _countryService.GetListAsync(filter);
                res = await _countryHelper.MergeDataList(res);
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
        public async Task<ActionResult<R_Data>>GetCountryById(int id)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _countryService.GetAsync(id);
                res = await _countryHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>> GetListCountry()
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                Expression<Func<Country, bool>> filter;
                filter = w => w.Status == 1;
                res = await _countryService.GetListAsync(filter);
                res = await _countryHelper.MergeDataList(res);
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
                res = await _countryService.Delete(id, updatedBy);
                res = await _countryHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>> Update(Country item)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _countryService.PutAsync(item.Id, item.Name, item.Remark, item.UpdatedBy, item.Timer);
                res = await _countryHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>> Create(Country item)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _countryService.PutAsync(item.Name);
                res = await _countryHelper.MergeData(res);
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
                res = await _countryService.PutAsync(ori.Id, ori.Status ?? 0, ori.UpdatedBy ?? 0, ori.Timer);
                res = await _countryHelper.MergeData(res);
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
