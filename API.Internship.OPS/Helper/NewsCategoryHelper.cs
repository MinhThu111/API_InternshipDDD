using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Reflection;
using API.Internship.Domain.Services;
using System.Linq.Expressions;

namespace API.Internship.OPS.Helper
{
    public interface INewsCategoryHelper
    {
        public Task<R_Data> MergeData(R_Data res);
        public Task<R_Data> MergeDataList(R_Data res);
        public Task<R_Data> MergeDynamicList(R_Data res);
    }
    public class NewsCategoryHelper: INewsCategoryHelper
    {
        private readonly ILogger<NewsCategoryHelper> _logger;

        private readonly INewsCategoryService _newscategoryService;

        public NewsCategoryHelper(ILogger<NewsCategoryHelper> logger, INewsCategoryService newscategoryService)
        {
            _logger = logger;
            _newscategoryService = newscategoryService;
        }

        public async Task<R_Data> MergeData(R_Data res)
        {
            try
            {
                if (res.result == 1 && res.data != null)
                {
                    Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
                    NewsCategory NewsCategoryObj = res.data;
                    Type myType = NewsCategoryObj.GetType();
                    IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                    foreach (PropertyInfo prop in props)
                    {
                        dict.Add(prop.Name, prop.GetValue(NewsCategoryObj));
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
                    List<NewsCategory> NewsCategoryObjs = res.data;
                    NewsCategoryObjs.ForEach(NewsCategoryObj =>
                    {
                        Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
                        Type myType = NewsCategoryObj.GetType();
                        IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                        foreach (PropertyInfo prop in props)
                        {
                            dict.Add(prop.Name, prop.GetValue(NewsCategoryObj));
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
                    List< NewsCategory> NewsCategoryObjs = res.data;

                    List<NewsCategory> lstparent = NewsCategoryObjs.Where(w => w.ParentId == 0).ToList();
                    lstparent.ForEach(async obj =>
                    {
                        Type myType = obj.GetType();
                        IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                        Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
                        foreach (PropertyInfo prop in props)
                        {
                            dict.Add(prop.Name, prop.GetValue(obj));
                        }
                        Expression<Func<NewsCategory, bool>> filter;
                        filter = w => w.Status == 1 && w.ParentId==obj.Id;
                        filter.Compile();
                        R_Data reschild = _newscategoryService.GetListAsync(filter).Result;
                        if (reschild.result == 1 && reschild.data != null)
                        {

                            dict.Add("ChildMenu", reschild.data);

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
    }
}
