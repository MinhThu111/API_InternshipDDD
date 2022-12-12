using API.Internship.Domain.Interfaces;
using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Linq.Expressions;

namespace API.Internship.Domain.Services;

public interface ISchoolInfoService
{
    Task<R_Data> GetAsync(int id);
    Task<R_Data> GetListAsync(Expression<Func<SchoolInfo, bool>> expression);
    Task<R_Data> PutAsync(string name, string nameen, int? addressid, string solgan, DateTime? establish);
    Task<R_Data> PutAsync(int id, string name, DateTime timer, string nameen, string solgan, int? updateby, DateTime? establish);
    Task<R_Data> Delete(int id, int? updatedBy);
}
public class SchoolInfoService: ISchoolInfoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SchoolInfoService> _logger;
    private readonly string refName = nameof(SchoolInfo);
    public SchoolInfoService(IUnitOfWork unitOfWork, ILogger<SchoolInfoService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<R_Data> GetAsync(int id)
    {
        error errObj = new error();
        R_Data res = new R_Data { result = 1, data = null, error = errObj };
        var categoryObj = await Task.FromResult<SchoolInfo>(new SchoolInfo());
        try
        {
            categoryObj = await _unitOfWork.SchoolInfoRepository.GetId(id);
            if (categoryObj == null)
            {
                errObj.message = "Load data is successful and do not data to show!";
            }
            else
            {
                res.data = categoryObj;
            }
        }
        catch (Exception ex)
        {
            res.result = 0;
            res.data = null;
            res.error = new error { code = 201, message = $"Exception: Xẩy ra lỗi khi đọc dữ liệu {ex}" };
        }
        return res;
    }
    public async Task<R_Data> GetListAsync(Expression<Func<SchoolInfo, bool>> expression)
    {
        error errObj = new error();
        R_Data res = new R_Data { result = 1, data = null, error = errObj };
        var lstObj = await Task.FromResult<List<SchoolInfo>>(new List<SchoolInfo>());
        try
        {
            lstObj = (await _unitOfWork.SchoolInfoRepository.ListAsync(expression)).ToList();
            if (lstObj == null)
            {
                errObj.message = "Load data is successful and do not data to show!";
            }
            else
            {
                res.data = lstObj;
            }
        }
        catch (Exception ex)
        {
            res.result = 0;
            res.data = null;
            res.error = new error { code = 201, message = $"Exception: Xẩy ra lỗi khi đọc dữ liệu {ex}" };
        }
        return res;
    }
    public async Task<R_Data> Delete(int id, int? updateby)
    {
        error errObj = new error();
        R_Data res = new R_Data { result = 1, data = null, error = errObj };
        var categoryObj = await Task.FromResult<SchoolInfo>(new SchoolInfo());
        try
        {
            Expression<Func<SchoolInfo, bool>> filter;
            filter = w => w.Id == id;
            categoryObj = _unitOfWork.SchoolInfoRepository.Find(filter);
            if (categoryObj == null)
            {
                res.result = 0;
                errObj.message = $"Không tìm thấy {id} để xóa.";
            }
            else
            {
                _unitOfWork.SchoolInfoRepository.Delete(categoryObj);
                var result = await _unitOfWork.CommitAsync();
                if (result > 0)
                {
                    categoryObj = await _unitOfWork.SchoolInfoRepository.GetId(id);
                    if (categoryObj == null)
                        errObj.message = "Đã xóa dữ liệu thành công.";
                }
            }
            res.data = categoryObj;
        }
        catch (Exception ex)
        {
            res.result = 0;
            res.data = null;
            res.error = new error { code = 201, message = $"Exception: Xẩy ra lỗi khi xóa dữ liệu {ex}" };
        }
        return res;
    }

    public async Task<R_Data> PutAsync(int id, string name, DateTime timer, string nameen, string solgan, int? updateby, DateTime? establish)
    {
        error errObj = new error();
        R_Data res = new R_Data { result = 1, data = null, error = errObj };
        var categoryObj = await Task.FromResult<SchoolInfo>(new SchoolInfo());
        var existSchoolInfo = await _unitOfWork.SchoolInfoRepository.GetId(id);
        if (existSchoolInfo == null)
        {
            throw new Exception($"Grade {id} không tìm thấy.");
        }
        if (existSchoolInfo.Timer > timer)
        {
            res.result = 0;
            res.data = null;
            res.error = new error() { code = 201, message = "Thông tin đã được cập nhật lại trước đó. Vui lòng hủy thao tác và thực hiện lại để dữ liệu đồng bộ!" };
            return res;
        }
        SchoolInfo item = new SchoolInfo()
        {
            Name = name,
            NameEn = nameen,
            Sologan = solgan,
            EstablishDate = establish,
            UpdatedBy = updateby,
            UpdatedAt = DateTime.Now
        };
        try
        {
            await _unitOfWork.SchoolInfoRepository.UpdateAsync(item);
            var result = await _unitOfWork.CommitAsync();
            if (result > 0)
            {
                categoryObj = await _unitOfWork.SchoolInfoRepository.GetId(item.Id);
                errObj.message = "Cập nhật dữ liệu thành công.";
            }
            res.data = categoryObj;
        }
        catch (Exception ex)
        {
            res.result = 0;
            res.data = null;
            res.error = new error { code = 201, message = $"Exception: Xẩy ra lỗi khi cập nhật dữ liệu {ex}" };
        }

        return res;
    }

    public async Task<R_Data> PutAsync(string name, string nameen, int? addressid, string solgan, DateTime? establish)
    {
        error errObj = new error();
        R_Data res = new R_Data { result = 1, data = null, error = errObj };
        var categoryObj = await Task.FromResult<SchoolInfo>(new SchoolInfo());
        var idMax = await _unitOfWork.SchoolInfoRepository.Max();
        if (idMax.code != 1)
            throw new Exception();

        SchoolInfo item = new SchoolInfo()
        {
            Id = idMax.data + 1,
            Name = name,
            NameEn = nameen,
            AddressId = addressid,
            Sologan = solgan,
            EstablishDate = establish,
            Status = 1,
            CreatedAt = DateTime.Now,
            Timer = DateTime.Now

        };

        try
        {
            _unitOfWork.SchoolInfoRepository.Add(item);
            var result = await _unitOfWork.CommitAsync();
            if (result > 0)
            {
                categoryObj = await _unitOfWork.SchoolInfoRepository.GetId(item.Id);
                errObj.message = "Thêm dữ liệu thành công.";
            }
            res.data = categoryObj;

        }
        catch (Exception ex)
        {
            res.result = 0;
            res.data = null;
            res.error = new error() { code = -1, message = $"Exeception: {ex.Message}" };
        }


        return res;
    }
}
