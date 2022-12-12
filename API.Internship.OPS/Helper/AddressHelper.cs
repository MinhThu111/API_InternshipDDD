using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Reflection;
using API.Internship.Domain.Services;

namespace API.Internship.OPS.Helper
{
    public interface IAddressHelper
    {
        public Task<R_Data> MergeData(R_Data res);
        public Task<R_Data> MergeDataList(R_Data res);
        public Task<R_Data> MergeDynamicList(R_Data res);
    }
    public class AddressHelper: IAddressHelper
    {
        private readonly ILogger<AddressHelper> _logger;

        private readonly ICountryService _countryService;
        private readonly IProvinceService _provinceService;
        private readonly IDistrictService _districtService;

        private readonly IWardService _wardService;
        public AddressHelper(ILogger<AddressHelper> logger, ICountryService countryService, IProvinceService provinceService, IDistrictService districtService, IWardService wardService)
        {
            _logger = logger;
            _countryService = countryService;
            _provinceService = provinceService;
            _districtService = districtService;
            _wardService = wardService;
        }

        public async Task<R_Data> MergeData(R_Data res)
        {
            try
            {
                if (res.result == 1 && res.data != null)
                {
                    Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
                    Address addressObj = res.data;
                    Type myType = addressObj.GetType();
                    IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                    foreach (PropertyInfo prop in props)
                    {
                        dict.Add(prop.Name, prop.GetValue(addressObj));
                    }

                    //Country obj
                    dict.Add("CountryObj", new Dictionary<string, dynamic>());
                    R_Data resCountry = _countryService.GetAsync((int)addressObj.CountryId).Result;
                    if (resCountry.result == 1 && resCountry.data != null)
                    {
                        Country countryitem = resCountry.data;
                        dict["CountryObj"] = new
                        {
                            countryitem.Id,
                            countryitem.Name,
                            countryitem.NameSlug
                        };
                    }

                    ////Province obj
                    dict.Add("ProvinceObj", new Dictionary<string, dynamic>());
                    R_Data resProvince = _provinceService.GetAsync(((int)addressObj.ProvinceId)).Result;
                    if (resProvince.result == 1 && resProvince.data != null)
                    {
                        Province provinceitem = resProvince.data;
                        dict["ProvinceObj"] = new
                        {
                            provinceitem.Id,
                            provinceitem.Name,
                            provinceitem.NameSlug,
                            provinceitem.CountryId
                        };
                    }

                    //Dictrict obj
                    dict.Add("District", new Dictionary<string, dynamic>());
                    R_Data resDistrict = _districtService.GetAsync(((int)addressObj.DistrictId)).Result;
                    if (resDistrict.result == 1 && resDistrict.data != null)
                    {
                        District districtitem = resDistrict.data;
                        dict["District"] = new
                        {
                            districtitem.Id,
                            districtitem.Name,
                            districtitem.ProvinceId
                        };
                    }

                    ////Ward obj
                    dict.Add("WardObj", new Dictionary<string, dynamic>());
                    R_Data resWard = _wardService.GetAsync(((int)addressObj.WardId)).Result;
                    if (resWard.result == 1 && resWard.data != null)
                    {
                        Ward warditem = resWard.data;
                        dict["WardObj"] = new
                        {
                            warditem.Id,
                            warditem.Name1,
                            warditem.DistrictId
                        };
                    }
                    res.data = dict;
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
        public async Task<R_Data> MergeDataList(R_Data res)
        {
            List<Dictionary<string, dynamic>> lstdict = new List<Dictionary<string, dynamic>>();
            try
            {
                if (res.result == 1 && res.data != null)
                {
                    List<Address> addressObjs = res.data;
                    addressObjs.ForEach(addressObj =>
                    {
                        Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
                        Type myType = addressObj.GetType();
                        IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                        foreach (PropertyInfo prop in props)
                        {
                            dict.Add(prop.Name, prop.GetValue(addressObj));
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

                    var addressObjs = res.data;
                    foreach (var addressObj in addressObjs)
                    {
                        Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
                        Type myType = addressObj.GetType();
                        IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                        foreach (PropertyInfo prop in props)
                        {
                            dict.Add(prop.Name, prop.GetValue(addressObj));
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
