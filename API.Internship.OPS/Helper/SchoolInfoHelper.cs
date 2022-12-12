using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Reflection;
using API.Internship.Domain.Services;
namespace API.Internship.OPS.Helper
{
    public interface ISchoolInfoHelper
    {
        public Task<R_Data> MergeData(R_Data res);
        public Task<R_Data> MergeDataList(R_Data res);
        public Task<R_Data> MergeDynamicList(R_Data res);
    }
    public class SchoolInfoHelper: ISchoolInfoHelper
    {
        private readonly ILogger<SchoolInfoHelper> _logger;

        private readonly IAddressService _addressService;
        private readonly ICountryService _countryService;
        private readonly IProvinceService _provinceService;
        private readonly IDistrictService _districtService;

        private readonly IWardService _wardService;
        public SchoolInfoHelper(ILogger<SchoolInfoHelper> logger, ICountryService countryService, IProvinceService provinceService, IDistrictService districtService, IWardService wardService, IAddressService addressService)
        {
            _logger = logger;
            _countryService = countryService;
            _provinceService = provinceService;
            _districtService = districtService;
            _wardService = wardService;
            _addressService = addressService;

        }
        public async Task<R_Data> MergeData(R_Data res)
        {
            try
            {
                Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
                if (res.result == 1 && res.data != null)
                {
                    SchoolInfo SchoolInfoObj = res.data;
                    Type myType = SchoolInfoObj.GetType();
                    IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                    foreach (PropertyInfo prop in props)
                    {
                        dict.Add(prop.Name, prop.GetValue(SchoolInfoObj));
                    }
                    //Address
                    dict.Add("AddressObj", new Dictionary<string, dynamic>());
                    R_Data resAddress = _addressService.GetAsync((int)SchoolInfoObj.AddressId).Result;
                    if (resAddress.result == 1 && resAddress.data != null)
                    {
                        Address addressitem = resAddress.data;
                        Dictionary<string, dynamic> dictAddress = new Dictionary<string, dynamic>();
                        Type addressType = addressitem.GetType();
                        IList<PropertyInfo> addressprops = new List<PropertyInfo>(addressType.GetProperties());
                        foreach (PropertyInfo prop in addressprops)
                        {
                            dictAddress.Add(prop.Name, prop.GetValue(addressitem));
                        }

                        //Country obj
                        dictAddress.Add("CountryObj", new Dictionary<string, dynamic>());
                        R_Data resCountry = _countryService.GetAsync((int)addressitem.CountryId).Result;
                        if (resCountry.result == 1 && resCountry.data != null)
                        {
                            Country countryitem = resCountry.data;
                            dictAddress["ScoreObj"] = new
                            {
                                countryitem.Id,
                                countryitem.Name
                            };
                        }
                        //ProvinceId obj
                        dictAddress.Add("ProvinceObj", new Dictionary<string, dynamic>());
                        R_Data resProvince = _provinceService.GetAsync((int)addressitem.ProvinceId).Result;
                        if (resProvince.result == 1 && resProvince.data != null)
                        {
                            Province provinceitem = resProvince.data;
                            dictAddress["ProvinceObj"] = new
                            {
                                provinceitem.Id,
                                provinceitem.Name,
                            };
                        }

                        //DistrictId obj
                        dictAddress.Add("DistrictObj", new Dictionary<string, dynamic>());
                        R_Data resDistric = _districtService.GetAsync((int)addressitem.DistrictId).Result;
                        if (resDistric.result == 1 && resDistric.data != null)
                        {
                            District districitem = resDistric.data;
                            dictAddress["DistrictObj"] = new
                            {
                                districitem.Id,
                                districitem.Name
                            };
                        }

                        //Ward obj
                        dictAddress.Add("WardObj", new Dictionary<string, dynamic>());
                        R_Data resWard = _wardService.GetAsync((int)addressitem.WardId).Result;
                        if (resWard.result == 1 && resWard.data != null)
                        {
                            Ward Warditem = resWard.data;
                            dictAddress["DistrictObj"] = new
                            {
                                Warditem.Id,
                                Warditem.Name1
                            };
                        }

                        dict["AddressObj"] = dictAddress;
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
                    List<SchoolInfo> SchoolInfoObjs = res.data;
                    SchoolInfoObjs.ForEach(SchoolInfoObj =>
                    {
                        Type myType = SchoolInfoObj.GetType();
                        IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                        Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
                        foreach (PropertyInfo prop in props)
                        {
                            dict.Add(prop.Name, prop.GetValue(SchoolInfoObj));
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

                    var SchoolInfoObjs = res.data;
                    foreach (var SchoolInfoObj in SchoolInfoObjs)
                    {
                        Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
                        Type myType = SchoolInfoObj.GetType();
                        IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                        foreach (PropertyInfo prop in props)
                        {
                            dict.Add(prop.Name, prop.GetValue(SchoolInfoObj));
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
