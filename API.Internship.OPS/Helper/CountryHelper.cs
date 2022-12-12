using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Reflection;
using API.Internship.Domain.Services;
namespace API.Internship.OPS.Helper
{
    public interface ICountryHelper
    {
        public Task<R_Data> MergeData(R_Data res);
        public Task<R_Data> MergeDataList(R_Data res);
        public Task<R_Data> MergeDynamicList(R_Data res);
    }
    public class CountryHelper : ICountryHelper
    {
        private readonly ILogger<CountryHelper> _logger;
        //private readonly ICountryService _countryService;
        //private readonly IProvinceService _provinceService;
        //private readonly IDistrictService _districtService;

        //private readonly IWardService _wardService;
        public CountryHelper(ILogger<CountryHelper> logger/*, ICountryService countryService, IProvinceService provinceService, IDistrictService districtService, IWardService wardService*/)
        {
            _logger = logger;
            //_countryService = countryService;
            //_provinceService = provinceService;
            //_districtService = districtService;
            //_wardService = wardService;
        }
        public async Task<R_Data> MergeData(R_Data res)
        {
            try
            {
                Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
                if (res.result == 1 && res.data != null)
                {
                    Country countryObj = res.data;
                    Type myType = countryObj.GetType();
                    IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                    foreach (PropertyInfo prop in props)
                    {
                        dict.Add(prop.Name, prop.GetValue(countryObj));
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
                    List<Country> countryObjs = res.data;
                    countryObjs.ForEach(countryObj =>
                    {
                        Type myType = countryObj.GetType();
                        IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                        Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
                        foreach (PropertyInfo prop in props)
                        {
                            dict.Add(prop.Name, prop.GetValue(countryObj));
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

                    var countryObjs = res.data;
                    foreach (var countryObj in countryObjs)
                    {
                        Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
                        Type myType = countryObj.GetType();
                        IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                        foreach (PropertyInfo prop in props)
                        {
                            dict.Add(prop.Name, prop.GetValue(countryObj));
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
