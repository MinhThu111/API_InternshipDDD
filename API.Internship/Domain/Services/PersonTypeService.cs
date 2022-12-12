using API.Internship.Domain.Interfaces;
using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Linq.Expressions;
namespace API.Internship.Domain.Services
{
    public interface IPersonTypeService
    {
        Task<R_Data> GetAsync(int id);
        Task<R_Data> GetListAsync(Expression<Func<PersonType, bool>> expression);
        Task<R_Data> PutAsync(int id, string name, string remark, int? status, DateTime timer, int? createby);
        Task<R_Data> PutAsync(string name);
        Task<R_Data> Delete(int id, int? updateby);
    }
    public class PersonTypeService: IPersonTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PersonTypeService> _logger;
        public PersonTypeService(IUnitOfWork unitOfWork, ILogger<PersonTypeService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<R_Data> Delete(int id, int? updateby)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<PersonType>(new PersonType());
            try
            {
                Expression<Func<PersonType, bool>> filter;
                filter = w => w.Id == id;
                categoryObj = _unitOfWork.PersonTypeRepository.Find(filter);
                if(categoryObj==null)
                {
                    res.result = 0;
                    errObj.message = $"Không tìm thấy {id} để xóa.";
                }
                else
                {
                    categoryObj.UpdatedBy = categoryObj.UpdatedBy;
                    _unitOfWork.PersonTypeRepository.Delete(categoryObj);
                    var result = await _unitOfWork.CommitAsync();
                    if(result>0)
                    {
                        categoryObj = await _unitOfWork.PersonTypeRepository.GetId(id);
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

        public async Task<R_Data> GetAsync(int id)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var categoryPersontype = await Task.FromResult<PersonType>(new PersonType());
            try
            {
                categoryPersontype = await _unitOfWork.PersonTypeRepository.GetId(id);
                if(categoryPersontype==null)
                {
                    errObj.message= "Load data is successful and do not data to show!";
                }
                else
                {
                    res.data = categoryPersontype;
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
        public async Task<R_Data> GetListAsync(Expression<Func<PersonType, bool>> expression)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var lstObj=await Task.FromResult<List<PersonType>>(new List<PersonType>());
            try
            {
                lstObj = (await _unitOfWork.PersonTypeRepository.ListAsync(expression)).ToList();
                if (lstObj.Count <= 0)
                    errObj.message = "Load data is successful and do not data to show!";
                else
                    res.data = lstObj;
            }
            catch (Exception ex)
            {
                res.result = 0;
                res.data = null;
                res.error = new error { code = 201, message = $"Exception: Xẩy ra lỗi khi đọc dữ liệu {ex}" };
            }
            return res;
        }
        public async Task<R_Data> PutAsync(int id, string name, string remark, int? status, DateTime timer, int? createby)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var categotyObj = await Task.FromResult<PersonType>(new PersonType());
            var existPersonType = await _unitOfWork.PersonTypeRepository.GetId(id);
            if (existPersonType == null)
            {
                throw new Exception($"Grade {id} không tìm thấy.");
            }
            if(existPersonType.Timer > timer)
            {
                res.result = 0;
                res.data = null;
                res.error = new error() { code = 201, message = "Thông tin đã được cập nhật lại trước đó. Vui lòng hủy thao tác và thực hiện lại để dữ liệu đồng bộ!" };
                return res;
            }

            PersonType item = new PersonType()
            {
                Id = id,
                Name = name,
                Remark = remark,
                Status = status,
                Timer = timer,
                CreatedBy = createby
            };

            try
            {
                await _unitOfWork.PersonTypeRepository.UpdateAsync(item);
                var result = await _unitOfWork.CommitAsync();
                if(result>0)
                {
                    categotyObj = await _unitOfWork.PersonTypeRepository.GetId(id);
                    errObj.message = "Cập nhật dữ liệu thành công.";
                }
                res.data = categotyObj;
            }
            catch (Exception ex)
            {
                res.result = 0;
                res.data = null;
                res.error = new error { code = 201, message = $"Exception: Xẩy ra lỗi khi cập nhật dữ liệu {ex}" };
            }

            return res;
            
        }
        public async Task<R_Data>PutAsync(string name)
        {
            error errObj = new error();
            R_Data res = new R_Data() { result = 1, data = null, error = errObj };
            try
            {
                var idMax = await _unitOfWork.PersonTypeRepository.Max();
                if (idMax.code != 1)
                    throw new Exception();
                PersonType item = new PersonType()
                {
                    Name = name,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Timer = DateTime.Now,
                    Status = 1,
                };
                item.Id = idMax.data + 1;


                _unitOfWork.PersonTypeRepository.Add(item);
                var result = await _unitOfWork.CommitAsync() ;
                if (result > 0)
                {
                    var rPersonType = await _unitOfWork.PersonTypeRepository.GetId(item.Id);
                    if (rPersonType!=null)
                    {
                        res.data = rPersonType;
                        errObj.message = "Thêm dữ liệu thành công!";
                    }
                }
                else
                {
                    res.result = 0;
                    errObj.code = 201;
                    errObj.message = "Thêm dữ liệu không thành công!";
                }

            }
            catch (Exception ex)
            {
                res.result = 0;
                res.data = null;
                res.error = new error() { code = -1, message = $"Exception: {ex.Message};" };
            }
            return res;
        }

    }
}
