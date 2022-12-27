using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Reflection;
using API.Internship.Domain.Services;
namespace API.Internship.OPS.Helper
{
    public interface ITeacherSubjectHelper
    {
        public Task<R_Data> MergeData(R_Data res);
        public Task<R_Data> MergeDataList(R_Data res);
        public Task<R_Data> MergeDynamicList(R_Data res);
    }
    public class TeacherSubjectHelper: ITeacherSubjectHelper
    {
        private readonly ILogger<TeacherSubjectHelper> _logger;
        private readonly ISubjectService _subjectService;
        private readonly IAddressService _addressService;
        private readonly ICountryService _countryService;
        private readonly IProvinceService _provinceService;
        private readonly IDistrictService _districtService;
        private readonly IPersonService _personService;
        private readonly INationalityService _nationalityService;
        private readonly IReligionService _religionService;
        private readonly IFolkService _folkService;
        private readonly IWardService _wardService;
        public TeacherSubjectHelper(ILogger<TeacherSubjectHelper> logger, IPersonService personService, IDistrictService districtService, IProvinceService provinceService, ICountryService countryService, IAddressService addressService, ISubjectService subjectService,  INationalityService nationalityService, IReligionService religionService, IFolkService folkService, IWardService wardService)
        {
            _logger = logger;
            _personService = personService;
            _countryService = countryService;
            _provinceService = provinceService;
            _districtService = districtService;
            _addressService = addressService;
            _subjectService = subjectService;
            _nationalityService = nationalityService;
            _religionService = religionService;
            _folkService = folkService;
            _wardService = wardService;
        }
        public async Task<R_Data> MergeData(R_Data res)
        {
            try
            {
                Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
                if (res.result == 1 && res.data != null)
                {
                    TeacherSubject TeacherSubjectObj = res.data;
                    Type myType = TeacherSubjectObj.GetType();
                    IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                    foreach (PropertyInfo prop in props)
                    {
                        dict.Add(prop.Name, prop.GetValue(TeacherSubjectObj));
                    }
                    //subject obj
                    dict.Add("SubjectObj", new Dictionary<string, dynamic>());
                    R_Data resSubject = _subjectService.GetAsync((int)TeacherSubjectObj.SubjectId).Result;
                    if (resSubject.result == 1 && resSubject.data != null)
                    {
                        Subject subjectitem = resSubject.data;
                        dict["SubjectObj"] = new
                        {
                            subjectitem.Id,
                            subjectitem.Name
                        };
                    }

                    //teachetid obj
                    dict.Add("TeacherObj", new Dictionary<string, dynamic>());
                    R_Data resTeacher = _personService.GetAsync((int)TeacherSubjectObj.TeacherId).Result;
                    if (resTeacher.result == 1 && resTeacher.data != null)
                    {
                        Person personitem = resTeacher.data;
                        Dictionary<string, dynamic> dictitem = new Dictionary<string, dynamic>();
                        Type PersonType = personitem.GetType();
                        IList<PropertyInfo> Personprops = new List<PropertyInfo>(PersonType.GetProperties());
                        foreach (PropertyInfo prop in Personprops)
                        {
                            dictitem.Add(prop.Name, prop.GetValue(personitem));
                        }


                        dictitem.Add("NationlityObj", new Dictionary<string, dynamic>());
                        R_Data resNationality = _nationalityService.GetAsync((int)personitem.NationalityId).Result;
                        if (resNationality.result == 1 && resNationality.data != null)
                        {
                            Nationality nationalityitem = resNationality.data;
                            dictitem["NationlityObj"] = new
                            {
                                nationalityitem.Id,
                                nationalityitem.Name
                            };
                        }
                        dictitem.Add("ReligionObj", new Dictionary<string, dynamic>());
                        R_Data resReligion = _religionService.GetAsync((int)personitem.ReligionId).Result;
                        if (resReligion.result == 1 && resReligion.data != null)
                        {
                            Religion religionitem = resReligion.data;
                            dictitem["ReligionObj"] = new
                            {
                                religionitem.Id,
                                religionitem.Name
                            };
                        }
                        dictitem.Add("FolkObj", new Dictionary<string, dynamic>());
                        R_Data resFolk = _folkService.GetAsync((int)personitem.FolkId).Result;
                        if (resFolk.result == 1 && resFolk.data != null)
                        {
                            Folk folkitem = resFolk.data;
                            dictitem["FolkObj"] = new
                            {
                                folkitem.Id,
                                folkitem.Name
                            };
                        }
                        dictitem.Add("AddressObj", new Dictionary<string, dynamic>());
                        R_Data resAddress = _addressService.GetAsync((int)personitem.AddressId).Result;
                        if (resAddress.result == 1 && resAddress.data != null)
                        {

                            Dictionary<string, dynamic> dictaddress = new Dictionary<string, dynamic>();
                            Address addressitem = resAddress.data;
                            Type AddressType = addressitem.GetType();
                            IList<PropertyInfo> Addressprops = new List<PropertyInfo>(AddressType.GetProperties());
                            foreach (PropertyInfo prop in Addressprops)
                            {
                                dictaddress.Add(prop.Name, prop.GetValue(addressitem));
                            }
                            dictaddress.Add("CountryObj", new Dictionary<string, dynamic>());
                            R_Data resCountry = _countryService.GetAsync((int)addressitem.CountryId).Result;
                            if (resCountry.result == 1 && resCountry.data != null)
                            {
                                Country countryitem = resCountry.data;
                                dictaddress["CountryObj"] = new
                                {
                                    countryitem.Id,
                                    countryitem.Name
                                };
                            }
                            dictaddress.Add("ProvinceObj", new Dictionary<string, dynamic>());
                            R_Data resProvince =_provinceService.GetAsync((int)addressitem.ProvinceId).Result;
                            if (resProvince.result == 1 && resProvince.data != null)
                            {
                                Province provinceitem = resProvince.data;
                                dictaddress["ProvinceObj"] = new
                                {
                                    provinceitem.Id,
                                    provinceitem.Name
                                };
                            }
                            dictaddress.Add("DistrictObj", new Dictionary<string, dynamic>());
                            R_Data resDistrict = _districtService.GetAsync((int)addressitem.DistrictId).Result;
                            if (resDistrict.result == 1 && resDistrict.data != null)
                            {
                                District districtitem = resDistrict.data;
                                dictaddress["DistrictObj"] = new
                                {
                                    districtitem.Id,
                                    districtitem.Name
                                };
                            }
                            dictaddress.Add("WardObj", new Dictionary<string, dynamic>());
                            R_Data resWard = _wardService.GetAsync((int)addressitem.WardId).Result;
                            if (resWard.result == 1 && resWard.data != null)
                            {
                                Ward warditem = resWard.data;
                                dictaddress["WardObj"] = new
                                {
                                    warditem.Id,
                                    warditem.Name
                                };
                            }
                            dictitem["AddressObj"] = dictaddress;

                        }

                        dict["TeacherObj"] = dictitem;
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
                    List<TeacherSubject> TeacherSubjectObjs = res.data;
                    TeacherSubjectObjs.ForEach(TeacherSubjectObj =>
                    {
                        Type myType = TeacherSubjectObj.GetType();
                        IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                        Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
                        foreach (PropertyInfo prop in props)
                        {
                            dict.Add(prop.Name, prop.GetValue(TeacherSubjectObj));
                        }
                        //subject obj
                        dict.Add("SubjectObj", new Dictionary<string, dynamic>());
                        R_Data resSubject = _subjectService.GetAsync((int)TeacherSubjectObj.SubjectId).Result;
                        if (resSubject.result == 1 && resSubject.data != null)
                        {
                            Subject subjectitem = resSubject.data;
                            dict["SubjectObj"] = new
                            {
                                subjectitem.Id,
                                subjectitem.Name
                            };
                        }

                        //teachetid obj
                        //dict.Add("TeacherObj", new Dictionary<string, dynamic>());
                        R_Data resTeacher = _personService.GetAsync((int)TeacherSubjectObj.TeacherId).Result;
                        if (resTeacher.result == 1 && resTeacher.data != null)
                        {
                            Person personitem = resTeacher.data;
                            Dictionary<string, dynamic> dictitem = new Dictionary<string, dynamic>();
                            Type PersonType = personitem.GetType();
                            IList<PropertyInfo> Personprops = new List<PropertyInfo>(PersonType.GetProperties());
                            foreach (PropertyInfo prop in Personprops)
                            {
                                dictitem.Add(prop.Name, prop.GetValue(personitem));
                            }
                            dict.Add("TeacherObj", dictitem);
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

                    var TeacherSubjectObjs = res.data;
                    foreach (var TeacherSubjectObj in TeacherSubjectObjs)
                    {
                        Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
                        Type myType = TeacherSubjectObj.GetType();
                        IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                        foreach (PropertyInfo prop in props)
                        {
                            dict.Add(prop.Name, prop.GetValue(TeacherSubjectObj));
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
