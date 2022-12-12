using API.Internship.Domain.Interfaces;
using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Linq.Expressions;

namespace API.Internship.Domain.Services;

public interface IGradeStudentService
{
    Task<R_Data> GetAsync(int id);
    Task<R_Data> GetListAsync(Expression<Func<GradeStudent, bool>> expression);
    Task<R_Data> Delete(int id, int? updatedBy);
    Task<R_Data> PutAsync(int id, int? gradeid, int? studentid, int? postionid, DateTime? timer, int? updateby);
    Task<R_Data> PutAsync(int? gradeid, int? studentid, int? postionid);
}
public class GradeStudentService: IGradeStudentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GradeStudentService> _logger;
    public GradeStudentService(IUnitOfWork unitOfWork, ILogger<GradeStudentService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task<R_Data> GetAsync(int id)
    {
        error errObj = new error();
        R_Data res = new R_Data { result = 1, data = null, error = errObj };
        var categoryObj = await Task.FromResult<GradeStudent>(new GradeStudent());
        try
        {
            categoryObj = await _unitOfWork.GradeStudentRepository.GetId(id);
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
    public async Task<R_Data> GetListAsync(Expression<Func<GradeStudent, bool>> expression)
    {
        error errObj = new error();
        R_Data res = new R_Data { result = 1, data = null, error = errObj };
        var lstObj = await Task.FromResult<List<GradeStudent>>(new List<GradeStudent>());
        try
        {
            lstObj = (await _unitOfWork.GradeStudentRepository.ListAsync(expression)).ToList();
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
        var categoryObj = await Task.FromResult<GradeStudent>(new GradeStudent());
        try
        {
            Expression<Func<GradeStudent, bool>> filter;
            filter = w => w.Id == id;
            categoryObj = _unitOfWork.GradeStudentRepository.Find(filter);
            if (categoryObj == null)
            {
                res.result = 0;
                errObj.message = $"Không tìm thấy {id} để xóa.";
            }
            else
            {
                _unitOfWork.GradeStudentRepository.Delete(categoryObj);
                var result = await _unitOfWork.CommitAsync();
                if (result > 0)
                {
                    categoryObj = await _unitOfWork.GradeStudentRepository.GetId(id);
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

    public async Task<R_Data> PutAsync(int id, int? gradeid, int? studentid, int? postionid, DateTime? timer, int? updateby)
    {
        error errObj = new error();
        R_Data res = new R_Data { result = 1, data = null, error = errObj };
        var categoryObj = await Task.FromResult<GradeStudent>(new GradeStudent());
        var existGradeStudent = await _unitOfWork.GradeStudentRepository.GetId(id);
        if (existGradeStudent == null)
        {
            throw new Exception($"Grade {id} không tìm thấy.");
        }
        if (existGradeStudent.Timer > timer)
        {
            res.result = 0;
            res.data = null;
            res.error = new error() { code = 201, message = "Thông tin đã được cập nhật lại trước đó. Vui lòng hủy thao tác và thực hiện lại để dữ liệu đồng bộ!" };
            return res;
        }
        GradeStudent item = new GradeStudent()
        {
            GradeId = gradeid,
            StudentId = studentid,
            PositionId = postionid,
            UpdatedBy = updateby,
            UpdatedAt = DateTime.Now
        };
        try
        {
            await _unitOfWork.GradeStudentRepository.UpdateAsync(item);
            var result = await _unitOfWork.CommitAsync();
            if (result > 0)
            {
                categoryObj = await _unitOfWork.GradeStudentRepository.GetId(item.Id);
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

    public async Task<R_Data> PutAsync(int? gradeid, int? studentid, int? postionid)
    {
        error errObj = new error();
        R_Data res = new R_Data { result = 1, data = null, error = errObj };
        var categoryObj = await Task.FromResult<GradeStudent>(new GradeStudent());
        var idMax = await _unitOfWork.GradeStudentRepository.Max();
        if (idMax.code != 1)
            throw new Exception();

        GradeStudent item = new GradeStudent()
        {
            Id = idMax.data + 1,
            GradeId = gradeid,
            StudentId = studentid,
            PositionId = postionid,
            Status = 1,
            CreatedAt = DateTime.Now,
            Timer = DateTime.Now

        };

        try
        {
            _unitOfWork.GradeStudentRepository.Add(item);
            var result = await _unitOfWork.CommitAsync();
            if (result > 0)
            {
                categoryObj = await _unitOfWork.GradeStudentRepository.GetId(item.Id);
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
