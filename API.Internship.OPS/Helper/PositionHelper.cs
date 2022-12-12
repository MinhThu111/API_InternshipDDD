using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Reflection;
using API.Internship.Domain.Services;

namespace API.Internship.OPS.Helper
{
    public interface IPositionHelper
    {
        public Task<R_Data> MergeData(R_Data res);
        public Task<R_Data> MergeDataList(R_Data res);
        public Task<R_Data> MergeDynamicList(R_Data res);
    }
    public class PositionHelper: IPositionHelper
    {
        private readonly ILogger<PositionHelper> _logger;
        public PositionHelper(ILogger<PositionHelper> logger)
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
                    Position PositionObj = res.data;
                    Type myType = PositionObj.GetType();
                    IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                    foreach (PropertyInfo prop in props)
                    {
                        dict.Add(prop.Name, prop.GetValue(PositionObj));
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
                    List<Position> PositionObjs = res.data;
                    PositionObjs.ForEach(PositionObj =>
                    {
                        Type myType = PositionObj.GetType();
                        IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                        Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
                        foreach (PropertyInfo prop in props)
                        {
                            dict.Add(prop.Name, prop.GetValue(PositionObj));
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

                    var PositionObjs = res.data;
                    foreach (var PositionObj in PositionObjs)
                    {
                        Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
                        Type myType = PositionObj.GetType();
                        IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                        foreach (PropertyInfo prop in props)
                        {
                            dict.Add(prop.Name, prop.GetValue(PositionObj));
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
