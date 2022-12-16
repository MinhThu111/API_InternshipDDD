using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Reflection;
using API.Internship.Domain.Services;
namespace API.Internship.OPS.Helper
{
    public interface IGradeStudentHelper
    {
        public Task<R_Data> MergeData(R_Data res);
        public Task<R_Data> MergeDataList(R_Data res);
        public Task<R_Data> MergeDynamicList(R_Data res);
    }
    public class GradeStudentHelper: IGradeStudentHelper
    {
        private readonly ILogger<GradeStudentHelper> _logger;

        private readonly ICountryService _countryService;
        private readonly IGradeService _gradeService;
        private readonly IPersonService _personService;
        private readonly INationalityService _nationalityService;
        private readonly IReligionService _religionService;
        private readonly IFolkService _folkService;
        private readonly IAddressService _addressService; 
        private readonly IProvinceService _provinceService;
        private readonly IDistrictService _districtService;
        private readonly IPositionService _positionService;

        private readonly IWardService _wardService;
        public GradeStudentHelper(ILogger<GradeStudentHelper> logger,IGradeService gradeService, IPersonService personService, INationalityService nationalityService, IReligionService religionService, IFolkService folkService, IAddressService addressService, IProvinceService provinceService, IDistrictService districtService, IWardService wardService, IPositionService positionService, ICountryService countryService)
        {
            _logger = logger;
            _gradeService = gradeService;
            _personService = personService;
            _nationalityService = nationalityService;
            _religionService = religionService;
            _folkService=folkService;
            _addressService = addressService;
            _provinceService = provinceService;
            _districtService = districtService;
            _wardService = wardService;
            _positionService = positionService;
            _countryService = countryService;
        }
        public async Task<R_Data> MergeData(R_Data res)
        {
            Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
            try
            {
                if (res.result == 1 && res.data != null)
                {
                    GradeStudent GradeStudentObj = res.data;
                    Type myType = GradeStudentObj.GetType();
                    IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                    foreach (PropertyInfo prop in props)
                    {
                        dict.Add(prop.Name, prop.GetValue(GradeStudentObj));
                    }
                    //Grade obj
                    dict.Add("GradeObj", new Dictionary<string, dynamic>());
                    R_Data resGrade = _gradeService.GetAsync((int)GradeStudentObj.GradeId).Result;
                    if (resGrade.result == 1 && resGrade.data != null)
                    {
                        Grade gradeitem = resGrade.data;
                        dict["GradeObj"] = new
                        {
                            gradeitem.Id,
                            gradeitem.TeacherId,
                            gradeitem.Name
                        };
                    }
                    dict.Add("StudentObj", new Dictionary<string, dynamic>());
                    R_Data resStudent = _personService.GetAsync((int)GradeStudentObj.StudentId).Result;
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
                                    Warditem.Name
                                };
                            }

                            dictStudent["AddressObj"] = dictAddress;
                        }

                        dict["StudentObj"] = dictStudent;

                        //position
                        dict.Add("PositionObj", new Dictionary<string, dynamic>());
                        R_Data resPosition = _positionService.GetAsync((int)GradeStudentObj.PositionId).Result;
                        if (resPosition.result == 1 && resPosition.data != null)
                        {
                            Position positionitem = resPosition.data;
                            dict["PositionObj"] = new
                            {
                                positionitem.Id,
                                positionitem.Name
                            };
                        }
                    }
                    res.data = dict;
                }
            }
            catch (Exception ex)
            {
                res.result = 0;
                res.data = null;
                res.error = new error()
                {
                    code = -1,
                    message = $"Exception: {ex.Message};"
                };
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
                    List<GradeStudent> GradeStudentObjs = res.data;
                    GradeStudentObjs.ForEach(GradeStudentObj =>
                    {
                        Type myType = GradeStudentObj.GetType();
                        IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                        Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
                        foreach (PropertyInfo prop in props)
                        {
                            dict.Add(prop.Name, prop.GetValue(GradeStudentObj));
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

                    var GradeStudentObjs = res.data;
                    foreach (var GradeStudentObj in GradeStudentObjs)
                    {
                        Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
                        Type myType = GradeStudentObj.GetType();
                        IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                        foreach (PropertyInfo prop in props)
                        {
                            dict.Add(prop.Name, prop.GetValue(GradeStudentObj));
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
