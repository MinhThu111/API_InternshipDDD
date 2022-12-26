using API.Internship.Domain.Interfaces;
using API.Internship.ResData;
using System.Linq.Expressions;
using API.Internship.Domain.Models;
namespace API.Internship.Domain.Services;

public interface IPositionService
{
    Task<R_Data> GetAsync(int id);
    Task<R_Data> GetListAsync(Expression<Func<Position, bool>> expression);
    Task<R_Data> PutAsync(string name, string positioncode, string remark);
    Task<R_Data> PutAsync(int id, string name, DateTime timer, string positioncode, int? updateby);
    Task<R_Data> Delete(int id, int? updatedBy);
}
public class PositionService: IPositionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<PositionService> _logger;
    public PositionService(IUnitOfWork unitOfWork, ILogger<PositionService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<R_Data> GetAsync(int id)
    {
        error errObj = new error();
        R_Data res = new R_Data { result = 1, data = null, error = errObj };
        var categoryObj = await Task.FromResult<Position>(new Position());
        try
        {
            categoryObj = await _unitOfWork.PositionRepository.GetId(id);
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
    public async Task<R_Data> GetListAsync(Expression<Func<Position, bool>> expression)
    {
        error errObj = new error();
        R_Data res = new R_Data { result = 1, data = null, error = errObj };
        var lstObj = await Task.FromResult<List<Position>>(new List<Position>());
        try
        {
            lstObj = (await _unitOfWork.PositionRepository.ListAsync(expression)).ToList();
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
        var categoryObj = await Task.FromResult<Position>(new Position());
        try
        {
            Expression<Func<Position, bool>> filter;
            filter = w => w.Id == id;
            categoryObj = _unitOfWork.PositionRepository.Find(filter);
            if (categoryObj == null)
            {
                res.result = 0;
                errObj.message = $"Không tìm thấy {id} để xóa.";
            }
            else
            {
                _unitOfWork.PositionRepository.Delete(categoryObj);
                var result = await _unitOfWork.CommitAsync();
                if (result > 0)
                {
                    categoryObj = await _unitOfWork.PositionRepository.GetId(id);
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
        return await Task.Run(() => res);
    }
    public async Task<R_Data> PutAsync(int id, string name, DateTime timer, string positioncode, int? updateby)
    {
        error errObj = new error();
        R_Data res = new R_Data { result = 1, data = null, error = errObj };
        var categoryObj = await Task.FromResult<Position>(new Position());
        var existPosition = await _unitOfWork.PositionRepository.GetId(id);
        if (existPosition == null)
        {
            throw new Exception($"Grade {id} không tìm thấy.");
        }
        if (existPosition.Timer > timer)
        {
            res.result = 0;
            res.data = null;
            res.error = new error() { code = 201, message = "Thông tin đã được cập nhật lại trước đó. Vui lòng hủy thao tác và thực hiện lại để dữ liệu đồng bộ!" };
            return res;
        }
        Position item = new Position()
        {
            Name = name,
            PositionCode = positioncode,
            UpdatedBy = updateby,
            UpdatedAt = DateTime.Now
        };
        try
        {
            await _unitOfWork.PositionRepository.UpdateAsync(item);
            var result = await _unitOfWork.CommitAsync();
            if (result > 0)
            {
                categoryObj = await _unitOfWork.PositionRepository.GetId(item.Id);
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

        return await Task.Run(() => res);
    }
    public async Task<R_Data> PutAsync(string name, string positioncode, string remark)
    {
        error errObj = new error();
        R_Data res = new R_Data { result = 1, data = null, error = errObj };
        var categoryObj = await Task.FromResult<Position>(new Position());

        var idMax = await _unitOfWork.PositionRepository.Max();
        if (idMax.code != 1)
            throw new Exception();

        Position item = new Position()
        {
            Id = idMax.data + 1,
            Name = name,
            PositionCode = positioncode,
            Remark= remark,
            Status = 1,
            CreatedAt = DateTime.Now,
            Timer = DateTime.Now

        };

        try
        {
            _unitOfWork.PositionRepository.Add(item);
            var result = await _unitOfWork.CommitAsync();
            if (result > 0)
            {
                categoryObj = await _unitOfWork.PositionRepository.GetId(item.Id);
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


        return await Task.Run(() => res);
    }
}
