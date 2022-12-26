using API.Internship.Domain.Models;
using API.Internship.Domain.Services;
using API.Internship.OPS.Helper;
using API.Internship.ResData;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Internship.OPS.Controllers;
[Route("[controller]/[Action]"), ApiController]
public class PersonTypeController : ControllerBase
    {
    private readonly ILogger<PersonTypeController> _logger;
    private readonly IPersonTypeService _persontypeService;
    private readonly IPersonTypeHelper _persontypeHelper;

    public PersonTypeController(ILogger<PersonTypeController> logger, IPersonTypeService persontypeService, IPersonTypeHelper persontypeHelper)
    {
        _logger = logger;
        _persontypeService = persontypeService;
        _persontypeHelper = persontypeHelper;
    }
    
    [HttpGet]
    public async Task<ActionResult<R_Data>> getlistPersonType()
    {
        R_Data res = new R_Data { result = 1, data = null, error = new error() };
        try
        {
            Expression<Func<PersonType, bool>> filter;
            filter = w => w.Status == 1;
            res = await _persontypeService.GetListAsync(filter);
            res = await _persontypeHelper.MergeDataList(res);
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
    public async Task<ActionResult<R_Data>>getPersonTypeById(int id)
    {
        R_Data res = new R_Data { result = 1, data = null, error = new error() };
        try
        {
            res = await _persontypeService.GetAsync(id);
            res = await _persontypeHelper.MergeData(res);
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
    public async Task<ActionResult<R_Data>>Update(PersonType item)
    {
        R_Data res = new R_Data { result = 1, data = null, error = new error() };
        try
        {
            res = await _persontypeService.PutAsync(item.Id, item.Name, item.Remark, item.Status, item.Timer, item.CreatedBy);
            //res = await _persontypeService.PutAsync(id, name, remark, status, timer, createby);
            res = await _persontypeHelper.MergeData(res);
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
    public async Task<ActionResult<R_Data>>Create(PersonType item)
    {
        R_Data res = new R_Data { result = 1, data = null, error = new error() };
        try
        {
            res = await _persontypeService.PutAsync(item.Name);
            res = await _persontypeHelper.MergeData(res);
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
    public async Task<ActionResult<R_Data>>Delete(int id, int? updateby)
    {
        R_Data res = new R_Data { result = 1, data = null, error = new error() };

        try
        {
            res = await _persontypeService.Delete(id, updateby);
            res = await _persontypeHelper.MergeData(res);
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

