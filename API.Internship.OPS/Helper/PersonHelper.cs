using API.Internship.Domain.Models;
using API.Internship.Domain.Services;
using API.Internship.ResData;
using System.Reflection;

namespace API.Internship.OPS.Helper
{
    public interface IPersonHelper
    {
        public Task<R_Data> MergeData(R_Data res);
        public Task<R_Data> MergeDataList(R_Data res);
        public Task<R_Data> MergeDynamicList(R_Data res);
    }
    public class PersonHelper : IPersonHelper
    {
        private readonly ILogger<PersonHelper> _logger;
        private readonly IPersonTypeService _persontypeService;
        private readonly ICountryService _countryService;
        private readonly IProvinceService _provinceService;
        private readonly IDistrictService _districtService;
        private readonly IWardService _wardService;
        private readonly IAddressService _addressService;
        private readonly INationalityService _nationalityService;
        private readonly IReligionService _religionService;
        private readonly IFolkService _folkService;

        public PersonHelper(ILogger<PersonHelper> logger, IPersonTypeService persontypeService, ICountryService countryService, IProvinceService provinceService, IDistrictService districtService, IWardService wardService, IAddressService addressService, INationalityService nationalityService, IReligionService religionService, IFolkService folkService)
        {
            _logger = logger;
            _persontypeService = persontypeService;
            _countryService = countryService;
            _provinceService = provinceService;
            _districtService = districtService;
            _wardService = wardService;
            _addressService = addressService;
            _nationalityService = nationalityService;
            _religionService = religionService;
            _folkService = folkService;
        }


        public async Task<R_Data> MergeData(R_Data res)
        {
            try
            {
                if (res.result == 1 && res.data != null)
                {
                    Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
                    Person personObj = res.data;
                    Type myType = personObj.GetType();
                    IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                    foreach (PropertyInfo prop in props)
                    {
                        dict.Add(prop.Name, prop.GetValue(personObj));
                    }
                    //persontype obj 
                    dict.Add("PersonTypeObj", new Dictionary<string, dynamic>());
                    R_Data resPersonType = _persontypeService.GetAsync((int)personObj.PersonTypeId).Result;
                    if (resPersonType.result == 1 && resPersonType.data != null)
                    {
                        PersonType Persontypeitem = resPersonType.data;
                        dict["PersonTypeObj"] = new
                        {
                            Persontypeitem.Id,
                            Persontypeitem.Name
                        };
                    }

                    //NationalityId obj
                    dict.Add("NationalityObj", new Dictionary<string, dynamic>());
                    R_Data resNationality = _nationalityService.GetAsync((int)personObj.NationalityId).Result;
                    if (resNationality.result == 1 && resNationality.data != null)
                    {
                        Nationality nationalityitem = resNationality.data;
                        dict["NationalityObj"] = new
                        {
                            nationalityitem.Id,
                            nationalityitem.Name,
                            nationalityitem.NameSlug
                        };
                    }

                    //ReligionId obj
                    dict.Add("ReligionObj", new Dictionary<string, dynamic>());
                    R_Data resReligion = _religionService.GetAsync((int)personObj.ReligionId).Result;
                    if (resReligion.result == 1 && resReligion.data != null)
                    {
                        Religion religionitem = resReligion.data;
                        dict["ReligionObj"] = new
                        {
                            religionitem.Id,
                            religionitem.Name,
                            religionitem.NameSlug
                        };
                    }

                    //FolkId obj
                    dict.Add("FolkObj", new Dictionary<string, dynamic>());
                    R_Data resFolk = _folkService.GetAsync((int)personObj.FolkId).Result;
                    if (resFolk.result == 1 && resFolk.data != null)
                    {
                        Folk folkitem = resFolk.data;
                        dict["FolkObj"] = new
                        {
                            folkitem.Id,
                            folkitem.Name,
                            folkitem.NameSlug
                        };
                    }

                    //AddressId obj
                    dict.Add("AddressObj", new Dictionary<string, dynamic>());
                    R_Data resAddress = _addressService.GetAsync((int)personObj.AddressId).Result;
                    if (resAddress.result == 1 && resAddress.data != null)
                    {
                        Address addressitem = resAddress.data;
                        Dictionary<string, dynamic> dictitem = new Dictionary<string, dynamic>();

                        Type AddressType = addressitem.GetType();
                        IList<PropertyInfo> Addressprops = new List<PropertyInfo>(AddressType.GetProperties());
                        foreach (PropertyInfo prop in Addressprops)
                        {
                            dictitem.Add(prop.Name, prop.GetValue(addressitem));
                        }

                        //country obj
                        dictitem.Add("CountryObj", new Dictionary<string, dynamic>());
                        R_Data resCountry = _countryService.GetAsync((int)addressitem.CountryId).Result;
                        if (resCountry.result == 1 && resCountry.data != null)
                        {
                            Country countryitem = resCountry.data;
                            dictitem["CountryObj"] = new
                            {
                                countryitem.Id,
                                countryitem.Name
                            };
                        }
                        //ProvinceId obj
                        dictitem.Add("ProvinceObj", new Dictionary<string, dynamic>());
                        R_Data resProvince = _provinceService.GetAsync((int)addressitem.ProvinceId).Result;
                        if (resProvince.result == 1 && resProvince.data != null)
                        {
                            Province provinceitem = resProvince.data;
                            dictitem["ProvinceObj"] = new
                            {
                                provinceitem.Id,
                                provinceitem.Name,
                            };
                        }

                        //DistrictId obj
                        dictitem.Add("DistrictObj", new Dictionary<string, dynamic>());
                        R_Data resDistric = _districtService.GetAsync((int)addressitem.DistrictId).Result;
                        if (resDistric.result == 1 && resDistric.data != null)
                        {
                            District districitem = resDistric.data;
                            dictitem["DistrictObj"] = new
                            {
                                districitem.Id,
                                districitem.Name
                            };
                        }

                        //Ward obj
                        dictitem.Add("WardObj", new Dictionary<string, dynamic>());
                        R_Data resWard = _wardService.GetAsync((int)addressitem.WardId).Result;
                        if (resWard.result == 1 && resWard.data != null)
                        {
                            Ward Warditem = resWard.data;
                            dictitem["DistrictObj"] = new
                            {
                                Warditem.Id,
                                Warditem.Name1
                            };
                        }
                        dict["AddressObj"] = dictitem;

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
                    List<Person> gradeObjs = res.data;
                    gradeObjs.ForEach(personObj =>
                    {
                        Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
                        Type myType = personObj.GetType();
                        IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                        foreach (PropertyInfo prop in props)
                        {
                            dict.Add(prop.Name, prop.GetValue(personObj));
                        }
                        //persontype obj 
                        dict.Add("PersonTypeObj", new Dictionary<string, dynamic>());
                        R_Data resPersonType = _persontypeService.GetAsync((int)personObj.PersonTypeId).Result;
                        if (resPersonType.result == 1 && resPersonType.data != null)
                        {
                            PersonType Persontypeitem = resPersonType.data;
                            dict["PersonTypeObj"] = new
                            {
                                Persontypeitem.Id,
                                Persontypeitem.Name
                            };
                        }

                        //NationalityId obj
                        dict.Add("NationalityObj", new Dictionary<string, dynamic>());
                        R_Data resNationality = _nationalityService.GetAsync((int)personObj.NationalityId).Result;
                        if (resNationality.result == 1 && resNationality.data != null)
                        {
                            Nationality nationalityitem = resNationality.data;
                            dict["NationalityObj"] = new
                            {
                                nationalityitem.Id,
                                nationalityitem.Name,
                                nationalityitem.NameSlug
                            };
                        }

                        //ReligionId obj
                        dict.Add("ReligionObj", new Dictionary<string, dynamic>());
                        R_Data resReligion = _religionService.GetAsync((int)personObj.ReligionId).Result;
                        if (resReligion.result == 1 && resReligion.data != null)
                        {
                            Religion religionitem = resReligion.data;
                            dict["ReligionObj"] = new
                            {
                                religionitem.Id,
                                religionitem.Name,
                                religionitem.NameSlug
                            };
                        }

                        //FolkId obj
                        dict.Add("FolkObj", new Dictionary<string, dynamic>());
                        R_Data resFolk = _folkService.GetAsync((int)personObj.FolkId).Result;
                        if (resFolk.result == 1 && resFolk.data != null)
                        {
                            Folk folkitem = resFolk.data;
                            dict["FolkObj"] = new
                            {
                                folkitem.Id,
                                folkitem.Name,
                                folkitem.NameSlug
                            };
                        }

                        //AddressId obj
                        dict.Add("AddressObj", new Dictionary<string, dynamic>());
                        R_Data resAddress = _addressService.GetAsync((int)personObj.AddressId).Result;
                        if (resAddress.result == 1 && resAddress.data != null)
                        {
                            Address addressitem = resAddress.data;
                            Dictionary<string, dynamic> dictitem = new Dictionary<string, dynamic>();

                            Type AddressType = addressitem.GetType();
                            IList<PropertyInfo> Addressprops = new List<PropertyInfo>(AddressType.GetProperties());
                            foreach (PropertyInfo prop in Addressprops)
                            {
                                dictitem.Add(prop.Name, prop.GetValue(addressitem));
                            }

                            //country obj
                            dictitem.Add("CountryObj", new Dictionary<string, dynamic>());
                            R_Data resCountry = _countryService.GetAsync((int)addressitem.CountryId).Result;
                            if (resCountry.result == 1 && resCountry.data != null)
                            {
                                Country countryitem = resCountry.data;
                                dictitem["CountryObj"] = new
                                {
                                    countryitem.Id,
                                    countryitem.Name
                                };
                            }
                            //ProvinceId obj
                            dictitem.Add("ProvinceObj", new Dictionary<string, dynamic>());
                            R_Data resProvince = _provinceService.GetAsync((int)addressitem.ProvinceId).Result;
                            if (resProvince.result == 1 && resProvince.data != null)
                            {
                                Province provinceitem = resProvince.data;
                                dictitem["ProvinceObj"] = new
                                {
                                    provinceitem.Id,
                                    provinceitem.Name,
                                };
                            }

                            //DistrictId obj
                            dictitem.Add("DistrictObj", new Dictionary<string, dynamic>());
                            R_Data resDistric = _districtService.GetAsync((int)addressitem.DistrictId).Result;
                            if (resDistric.result == 1 && resDistric.data != null)
                            {
                                District districitem = resDistric.data;
                                dictitem["DistrictObj"] = new
                                {
                                    districitem.Id,
                                    districitem.Name
                                };
                            }

                            //Ward obj
                            dictitem.Add("WardObj", new Dictionary<string, dynamic>());
                            R_Data resWard = _wardService.GetAsync((int)addressitem.WardId).Result;
                            if (resWard.result == 1 && resWard.data != null)
                            {
                                Ward Warditem = resWard.data;
                                dictitem["DistrictObj"] = new
                                {
                                    Warditem.Id,
                                    Warditem.Name1
                                };
                            }
                            dict["AddressObj"] = dictitem;

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

                    var personObjs = res.data;
                    foreach (var personObj in personObjs)
                    {
                        Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
                        Type myType = personObj.GetType();
                        IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                        foreach (PropertyInfo prop in props)
                        {
                            dict.Add(prop.Name, prop.GetValue(personObj));
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
