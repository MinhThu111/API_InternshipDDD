using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Reflection;
using API.Internship.Domain.Services;
namespace API.Internship.OPS.Helper
{
    public interface IDistrictHelper
    {
        public Task<R_Data> MergeData(R_Data res);
        public Task<R_Data> MergeDataList(R_Data res);
        public Task<R_Data> MergeDynamicList(R_Data res);
    }
    public class DistrictHelper: IDistrictHelper
    {
        private readonly ILogger<DistrictHelper> _logger;
        private readonly IProvinceService _provinceService;
        public DistrictHelper(ILogger<DistrictHelper> logger, IProvinceService provinceService)
        {
            _logger = logger;
            _provinceService = provinceService;
        }
        public async Task<R_Data> MergeData(R_Data res)
        {
            try
            {
                Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
                if (res.result == 1 && res.data != null)
                {
                    District districtObj = res.data;
                    Type myType = districtObj.GetType();
                    IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                    foreach (PropertyInfo prop in props)
                    {
                        dict.Add(prop.Name, prop.GetValue(districtObj));
                    }

                    //province obj
                    dict.Add("ProvinceObj", new Dictionary<string, dynamic>());
                    R_Data resDistrict = _provinceService.GetAsync((int)districtObj.ProvinceId).Result;
                    if (resDistrict.result == 1 && resDistrict.data != null)
                    {
                        Province provinceObj = resDistrict.data;
                        Type provinceType = provinceObj.GetType();
                        Dictionary<string, dynamic> dictProvince = new Dictionary<string, dynamic>();
                        IList<PropertyInfo> provinceprops = new List<PropertyInfo>(provinceType.GetProperties());
                        foreach (PropertyInfo prop in provinceprops)
                        {
                            dictProvince.Add(prop.Name, prop.GetValue(provinceObj));
                        }
                        dict["ProvinceObj"] = dictProvince;
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
                    List<District> districtObjs = res.data;
                    districtObjs.ForEach(districtObj =>
                    {
                        Type myType = districtObj.GetType();
                        IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                        Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
                        foreach (PropertyInfo prop in props)
                        {
                            dict.Add(prop.Name, prop.GetValue(districtObj));
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

                    var districtObjs = res.data;
                    foreach (var districtObj in districtObjs)
                    {
                        Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
                        Type myType = districtObj.GetType();
                        IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                        foreach (PropertyInfo prop in props)
                        {
                            dict.Add(prop.Name, prop.GetValue(districtObj));
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
