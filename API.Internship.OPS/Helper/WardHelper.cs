using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Reflection;
using API.Internship.Domain.Services;

namespace API.Internship.OPS.Helper
{
    public interface IWardHelper { 
        public Task<R_Data> MergeData(R_Data res);
        public Task<R_Data> MergeDataList(R_Data res);
        public Task<R_Data> MergeDynamicList(R_Data res);
    }
    public class WardHelper:IWardHelper
    {
    private readonly ILogger<WardHelper> _logger;
        private readonly IDistrictService _districtService;
        public WardHelper(ILogger<WardHelper> logger, IDistrictService districtService)
    {
        _logger = logger;
            _districtService = districtService;
    }
    public async Task<R_Data> MergeData(R_Data res)
    {
        try
        {
            Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
            if (res.result == 1 && res.data != null)
            {
                Ward wardObj = res.data;
                Type myType = wardObj.GetType();
                IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                foreach (PropertyInfo prop in props)
                {
                    dict.Add(prop.Name, prop.GetValue(wardObj));
                }
                    dict.Add("DistrictObj", new Dictionary<string, dynamic>());
                    R_Data resDistrict = _districtService.GetAsync((int)wardObj.DistrictId).Result;
                    if(resDistrict.result==1 && resDistrict.data!=null)
                    {
                        District districObj = resDistrict.data;
                        Type districType= districObj.GetType();
                        Dictionary<string, dynamic> dictDistrict = new Dictionary<string, dynamic>();
                        IList<PropertyInfo> districtprops = new List<PropertyInfo>(districType.GetProperties());
                        foreach(PropertyInfo prop in districtprops)
                        {
                            dictDistrict.Add(prop.Name, prop.GetValue(districObj));
                        }
                        dict["DistrictObj"] = dictDistrict;
                    }
            }
            res.data = dict;
        }
        catch (Exception ex)
        {
            res.result = 0;
            res.data = null;
            res.error = new error() { code = -1, message = $"Exeception: {ex.Message}" };
        }
        return await Task.Run(() => res);
    }

    public async Task<R_Data> MergeDataList(R_Data res)
    {
        List<Dictionary<string, dynamic>> lstdict = new List<Dictionary<string, dynamic>>();

        try
        {
            if (res.result == 1 && res.data != null)
            {
                List<Ward> wardObjs = res.data;
                wardObjs.ForEach(wardObj =>
                {
                    Type myType = wardObj.GetType();
                    IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                    Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
                    foreach (PropertyInfo prop in props)
                    {
                        dict.Add(prop.Name, prop.GetValue(wardObj));
                    }
                    lstdict.Add(dict);
                });
                res.data = lstdict;
            }
        }
        catch (Exception ex)
        {
            res.result = 0;
            res.data = null;
            res.error = new error() { code = -1, message = $"Exeception: {ex.Message}" };
        }
        return await Task.Run(() => res);
    }

    public async Task<R_Data> MergeDynamicList(R_Data res)
    {
        List<Dictionary<string, dynamic>> lstdict = new List<Dictionary<string, dynamic>>();
        try
        {
            if (res.result == 1 && res.data != null)
            {

                var wardObjs = res.data;
                foreach (var wardObj in wardObjs)
                {
                    Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
                    Type myType = wardObj.GetType();
                    IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                    foreach (PropertyInfo prop in props)
                    {
                        dict.Add(prop.Name, prop.GetValue(wardObj));
                    }
                    lstdict.Add(dict);
                }
                res.data = lstdict;
            }
        }
        catch (Exception ex)
        {
            res.result = 0;
            res.data = null;
            res.error = new error() { code = -1, message = $"Exeception: {ex.Message}" };
        }
        return await Task.Run(() => res);
    }
}
}
