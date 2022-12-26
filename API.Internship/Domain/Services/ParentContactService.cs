using API.Internship.Domain.Interfaces;
using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Linq.Expressions;

namespace API.Internship.Domain.Services;

public interface IParentContactService
{
    Task<R_Data> GetAsync(int id);
    Task<R_Data> GetListAsync(Expression<Func<ParentContact, bool>> expression);
    Task<R_Data> Delete(int id, int? updatedBy);
    Task<R_Data> PutAsync(int id, string firstname, string lastname, DateTime timer, int addressid, string phone);
    Task<R_Data> PutAsync(string fsname, string lsname, int addressid, string phone, string email);
}
public class ParentContactService: IParentContactService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ParentContactService> _logger;
    public ParentContactService(IUnitOfWork unitOfWork, ILogger<ParentContactService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task<R_Data> GetAsync(int id)
    {
        error errObj = new error();
        R_Data res = new R_Data { result = 1, data = null, error = errObj };
        var categoryObj = await Task.FromResult<ParentContact>(new ParentContact());
        try
        {
            categoryObj = await _unitOfWork.ParentContactRepository.GetId(id);
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
    public async Task<R_Data> GetListAsync(Expression<Func<ParentContact, bool>> expression)
    {
        error errObj = new error();
        R_Data res = new R_Data { result = 1, data = null, error = errObj };
        var lstObj = await Task.FromResult<List<ParentContact>>(new List<ParentContact>());
        try
        {
            lstObj = (await _unitOfWork.ParentContactRepository.ListAsync(expression)).ToList();
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
        var categoryObj = await Task.FromResult<ParentContact>(new ParentContact());
        try
        {
            Expression<Func<ParentContact, bool>> filter;
            filter = w => w.Id == id;
            categoryObj = _unitOfWork.ParentContactRepository.Find(filter);
            if (categoryObj == null)
            {
                res.result = 0;
                errObj.message = $"Không tìm thấy {id} để xóa.";
            }
            else
            {
                _unitOfWork.ParentContactRepository.Delete(categoryObj);
                var result = await _unitOfWork.CommitAsync();
                if (result > 0)
                {
                    categoryObj = await _unitOfWork.ParentContactRepository.GetId(id);
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
    public async Task<R_Data> PutAsync(int id, string firstname, string lastname, DateTime timer, int addressid, string phone)
    {
        error errObj = new error();
        R_Data res = new R_Data { result = 1, data = null, error = errObj };
        var categoryObj = await Task.FromResult<ParentContact>(new ParentContact());
        var existParentContact = await _unitOfWork.ParentContactRepository.GetId(id);
        if (existParentContact == null)
        {
            throw new Exception($"Grade {id} không tìm thấy.");
        }
        if (existParentContact.Timer > timer)
        {
            res.result = 0;
            res.data = null;
            res.error = new error() { code = 201, message = "Thông tin đã được cập nhật lại trước đó. Vui lòng hủy thao tác và thực hiện lại để dữ liệu đồng bộ!" };
            return res;
        }
        ParentContact item = new ParentContact()
        {
            FirstName = firstname,
            LastName = lastname,
            AddressId = addressid,
            PhoneNumber = phone,
            UpdatedAt = DateTime.Now
        };
        try
        {
            await _unitOfWork.ParentContactRepository.UpdateAsync(item);
            var result = await _unitOfWork.CommitAsync();
            if (result > 0)
            {
                categoryObj = await _unitOfWork.ParentContactRepository.GetId(item.Id);
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
    public async Task<R_Data> PutAsync(string fsname, string lsname, int addressid, string phone, string email)
    {
        error errObj = new error();
        R_Data res = new R_Data { result = 1, data = null, error = errObj };
        var categoryObj = await Task.FromResult<ParentContact>(new ParentContact());
        var idMax = await _unitOfWork.ParentContactRepository.Max();
        if (idMax.code != 1)
            throw new Exception();

        ParentContact item = new ParentContact()
        {
            Id = idMax.data + 1,
            FirstName = fsname,
            LastName = lsname,
            AddressId = addressid,
            PhoneNumber = phone,
            Email = email,
            Timer = DateTime.Now,
            CreatedAt = DateTime.Now,
            Status = 1,

        };

        try
        {
            _unitOfWork.ParentContactRepository.Add(item);
            var result = await _unitOfWork.CommitAsync();
            if (result > 0)
            {
                categoryObj = await _unitOfWork.ParentContactRepository.GetId(item.Id);
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
