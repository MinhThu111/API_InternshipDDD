using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Reflection;
using API.Internship.Domain.Services;
namespace API.Internship.OPS.Helper
{
    public interface INewsHelper
    {
        public Task<R_Data> MergeData(R_Data res);
        public Task<R_Data> MergeDataList(R_Data res);
        public Task<R_Data> MergeDynamicList(R_Data res);
    }
    public class NewsHelper: INewsHelper
    {
        private readonly ILogger<NewsHelper> _logger;
        private readonly INewsCategoryService _newsCategoryCategoryService;
        public NewsHelper(ILogger<NewsHelper> logger,INewsCategoryService newsCategoryCategoryService)
        {
            _logger = logger;
            _newsCategoryCategoryService=newsCategoryCategoryService;
        }
        public async Task<R_Data> MergeData(R_Data res)
        {
            try
            {
                if (res.result == 1 && res.data != null)
                {
                    Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
                    News NewsObj = res.data;
                    Type myType = NewsObj.GetType();
                    IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                    foreach (PropertyInfo prop in props)
                    {
                        dict.Add(prop.Name, prop.GetValue(NewsObj));
                    }

                    //NewCategoryObj
                    dict.Add("newscategoryObj", new Dictionary<string, dynamic>());
                    R_Data resnewscategory = _newsCategoryCategoryService.GetAsync((int)NewsObj.NewsCategoryId).Result;
                    if (resnewscategory.result == 1 && resnewscategory.data != null)
                    {
                        NewsCategory newscategoryitem = resnewscategory.data;
                        dict["newscategoryObj"] = new
                        {
                            newscategoryitem.Id,
                            newscategoryitem.Name,
                            newscategoryitem.Type

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
                    List<News> NewsObjs = res.data;
                    NewsObjs.ForEach(NewsObj =>
                    {
                        Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
                        Type myType = NewsObj.GetType();
                        IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                        foreach (PropertyInfo prop in props)
                        {
                            dict.Add(prop.Name, prop.GetValue(NewsObj));
                        }
                        dict.Add("newscategoryObj", new Dictionary<string, dynamic>());
                        R_Data resnewscategory = _newsCategoryCategoryService.GetAsync((int)NewsObj.NewsCategoryId).Result;
                        if (resnewscategory.result == 1 && resnewscategory.data != null)
                        {
                            NewsCategory newscategoryitem = resnewscategory.data;
                            dict["newscategoryObj"] = new
                            {
                                newscategoryitem.Id,
                                newscategoryitem.Name,
                                newscategoryitem.Type

                            };
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

                    var NewsObjs = res.data;
                    foreach (var NewsObj in NewsObjs)
                    {
                        Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
                        Type myType = NewsObj.GetType();
                        IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                        foreach (PropertyInfo prop in props)
                        {
                            dict.Add(prop.Name, prop.GetValue(NewsObj));
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
