using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Reflection;
using API.Internship.Domain.Services;
namespace API.Internship.OPS.Helper
{
    public interface IScoreHelper
    {
        public Task<R_Data> MergeData(R_Data res);
        public Task<R_Data> MergeDataList(R_Data res);
        public Task<R_Data> MergeDynamicList(R_Data res);
    }
    public class ScoreHelper: IScoreHelper
    {
        private readonly ILogger<ScoreHelper> _logger;
        private readonly IScoreTypeService _scoreTypeService;
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

        public ScoreHelper(ILogger<ScoreHelper> logger, IPersonService personService,IDistrictService districtService, IProvinceService provinceService, ICountryService countryService, IAddressService addressService, ISubjectService subjectService, IScoreTypeService scoreTypeService, INationalityService nationalityService, IReligionService religionService, IFolkService folkService, IWardService wardService)
        {
            _logger = logger;
            _personService = personService;
            _countryService = countryService;
            _provinceService = provinceService;
            _districtService = districtService;
            _addressService = addressService;
            _subjectService = subjectService;
            _scoreTypeService = scoreTypeService;
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
                    Score ScoreObj = res.data;
                    Type myType = ScoreObj.GetType();
                    IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                    foreach (PropertyInfo prop in props)
                    {
                        dict.Add(prop.Name, prop.GetValue(ScoreObj));
                    }

                    //score type
                    dict.Add("ScoreTypeObj", new Dictionary<string, dynamic>());
                    R_Data resScoreType = _scoreTypeService.GetAsync((int)ScoreObj.ScoreTypeId).Result;
                    if (resScoreType.result == 1 && resScoreType.data != null)
                    {
                        ScoreType scoretypeitem = resScoreType.data;
                        dict["ScoreTypeObj"] = new
                        {
                            scoretypeitem.Id,
                            scoretypeitem.Name
                        };
                    }

                    //Subject 
                    dict.Add("SubjectObj", new Dictionary<string, dynamic>());
                    R_Data resSubject = _subjectService.GetAsync((int)ScoreObj.SubjectId).Result;
                    if (resSubject.result == 1 && resSubject.data != null)
                    {
                        Subject subjectitem = resSubject.data;
                        dict["SubjectObj"] = new
                        {
                            subjectitem.Id,
                            subjectitem.Name
                        };
                    }

                    //Student
                    dict.Add("StudentObj", new Dictionary<string, dynamic>());
                    R_Data resStudent = _personService.GetAsync((int)ScoreObj.StudentId).Result;
                    if (resStudent.result == 1 && resStudent.data != null)
                    {
                        Person studentitem = resStudent.data;
                        Dictionary<string, dynamic> dictStudent = new Dictionary<string, dynamic>();
                        Type studentType = studentitem.GetType();
                        IList<PropertyInfo> studentprops = new List<PropertyInfo>(studentType.GetProperties());
                        foreach (PropertyInfo prop in studentprops)
                        {
                            dictStudent.Add(prop.Name, prop.GetValue(studentitem));
                        }

                        //Nationality
                        dictStudent.Add("NationlityObj", new Dictionary<string, dynamic>());
                        R_Data resNationality = _nationalityService.GetAsync((int)studentitem.NationalityId).Result;
                        if (resNationality.result == 1 && resNationality.data != null)
                        {
                            Nationality nationalityitem = resNationality.data;
                            dictStudent["NationlityObj"] = new
                            {
                                nationalityitem.Id,
                                nationalityitem.Name
                            };
                        }

                        //Religion
                        dictStudent.Add("ReligionObj", new Dictionary<string, dynamic>());
                        R_Data resReligion = _religionService.GetAsync((int)studentitem.ReligionId).Result;
                        if (resReligion.result == 1 && resReligion.data != null)
                        {
                            Religion religionitem = resReligion.data;
                            dictStudent["ReligionObj"] = new
                            {
                                religionitem.Id,
                                religionitem.Name
                            };
                        }

                        //Folk
                        dictStudent.Add("FolkObj", new Dictionary<string, dynamic>());
                        R_Data resFolk = _folkService.GetAsync((int)studentitem.FolkId).Result;
                        if (resFolk.result == 1 && resFolk.data != null)
                        {
                            Folk folkitem = resFolk.data;
                            dictStudent["FolkObj"] = new
                            {
                                folkitem.Id,
                                folkitem.Name
                            };
                        }

                        //Address
                        dictStudent.Add("AddressObj", new Dictionary<string, dynamic>());
                        R_Data resAddress = _addressService.GetAsync((int)studentitem.AddressId).Result;
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

                            //CountryObj
                            dictAddress.Add("CountryObj", new Dictionary<string, dynamic>());
                            R_Data resCountry =_countryService.GetAsync((int)addressitem.CountryId).Result;
                            if (resCountry.result == 1 && resCountry.data != null)
                            {
                                Country countryitem = resCountry.data;
                                dictAddress["CountryObj"] = new
                                {
                                    countryitem.Id,
                                    countryitem.Name
                                };
                            }
                            //ProvinceId obj
                            dictAddress.Add("ProvinceObj", new Dictionary<string, dynamic>());
                            R_Data resProvince =_provinceService.GetAsync((int)addressitem.ProvinceId).Result;
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
                            R_Data resDistric =_districtService.GetAsync((int)addressitem.DistrictId).Result;
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
                            R_Data resWard =_wardService.GetAsync((int)addressitem.WardId).Result;
                            if (resWard.result == 1 && resWard.data != null)
                            {
                                Ward Warditem = resWard.data;
                                dictAddress["DistrictObj"] = new
                                {
                                    Warditem.Id,
                                    Warditem.Name                                };
                            }

                            dictStudent["AddressObj"] = dictAddress;
                        }

                        dict["StudentObj"] = dictStudent;
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
                    List<Score> ScoreObjs = res.data;
                    ScoreObjs.ForEach(ScoreObj =>
                    {
                        Type myType = ScoreObj.GetType();
                        IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                        Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
                        foreach (PropertyInfo prop in props)
                        {
                            dict.Add(prop.Name, prop.GetValue(ScoreObj));
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

                    var ScoreObjs = res.data;
                    foreach (var ScoreObj in ScoreObjs)
                    {
                        Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
                        Type myType = ScoreObj.GetType();
                        IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                        foreach (PropertyInfo prop in props)
                        {
                            dict.Add(prop.Name, prop.GetValue(ScoreObj));
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
