
using API.Internship.Domain.Models;
using API.Internship.Domain.Services;
using API.Internship.OPS.Helper;
using API.Internship.ResData;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Internship.OPS.Controllers
{
    [Route("[controller]/[Action]"), ApiController]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;
        private readonly IPersonService _personService;
        private readonly IPersonHelper _personHelper;

        public PersonController(ILogger<PersonController> logger, IPersonService gradeService, IPersonHelper gradeHelper)
        {
            _logger = logger;
            _personService = gradeService;
            _personHelper = gradeHelper;
        }

        [HttpGet]
        public async Task<ActionResult<R_Data>> getListPersonBySequenceStatus(string sequenceStatus)
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
                Expression<Func<Person, bool>> filter;
                filter = w => lstStatus.Contains(w.Status);
                filter.Compile();
                res = await _personService.GetListAsync(filter);
                res = await _personHelper.MergeDataList(res);
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
        public async Task<ActionResult<R_Data>> getPersonById(int id)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _personService.GetAsync(id);
                res = await _personHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>> getListPerson()
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                Expression<Func<Person, bool>> filter;
                filter = w => w.Status == 1;
                filter.Compile();
                res = await _personService.GetListAsync(filter);
                res = await _personHelper.MergeDataList(res);
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
                res = await _personService.Delete(id, updatedBy);
                res = await _personHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>>Update(Person item)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _personService.PutAsync(item.Id, item.FirstName, item.LastName, item.Gender??0, item.PersonTypeId, item.Timer, item.Status??0, item.AddressId??0, item.PhoneNumber);
                res = await _personHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>> UpdateStatus(Person ori)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _personService.PutAsync(ori.Id, ori.Status ?? 0, ori.UpdatedBy ?? 0, ori.Timer);
                res = await _personHelper.MergeData(res);
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
        public async Task<ActionResult<R_Data>>Create(Person item)
        {
            R_Data res = new R_Data { result = 1, data = null, error = new error() };
            try
            {
                res = await _personService.PutAsync(item.FirstName, item.LastName, item.PersonTypeId, item.Birthday, item.Gender, item.NationalityId,item.ReligionId, item.FolkId, item.AddressId, item.PhoneNumber, item.Email);
                res = await _personHelper.MergeData(res);
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
