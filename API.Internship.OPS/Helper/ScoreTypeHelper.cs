using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Reflection;
using API.Internship.Domain.Services;
namespace API.Internship.OPS.Helper
{
    public interface IScoreTypeHelper
    {
        public Task<R_Data> MergeData(R_Data res);
        public Task<R_Data> MergeDataList(R_Data res);
        public Task<R_Data> MergeDynamicList(R_Data res);
    }
    public class ScoreTypeHelper: IScoreTypeHelper
    {
        private readonly ILogger<ScoreTypeHelper> _logger;
        public ScoreTypeHelper(ILogger<ScoreTypeHelper> logger)
        {
            _logger = logger;
        }
        public async Task<R_Data> MergeData(R_Data res)
        {
            try
            {
                Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
                if (res.result == 1 && res.data != null)
                {
                    ScoreType ScoreTypeObj = res.data;
                    Type myType = ScoreTypeObj.GetType();
                    IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                    foreach (PropertyInfo prop in props)
                    {
                        dict.Add(prop.Name, prop.GetValue(ScoreTypeObj));
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
                    List<ScoreType> ScoreTypeObjs = res.data;
                    ScoreTypeObjs.ForEach(ScoreTypeObj =>
                    {
                        Type myType = ScoreTypeObj.GetType();
                        IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                        Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
                        foreach (PropertyInfo prop in props)
                        {
                            dict.Add(prop.Name, prop.GetValue(ScoreTypeObj));
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

                    var ScoreTypeObjs = res.data;
                    foreach (var ScoreTypeObj in ScoreTypeObjs)
                    {
                        Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
                        Type myType = ScoreTypeObj.GetType();
                        IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                        foreach (PropertyInfo prop in props)
                        {
                            dict.Add(prop.Name, prop.GetValue(ScoreTypeObj));
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
