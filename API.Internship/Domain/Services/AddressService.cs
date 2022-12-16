using API.Internship.Domain.Interfaces;
using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Linq.Expressions;
namespace API.Internship.Domain.Services
{
    public interface IAddressService
    {
        Task<R_Data> GetAsync(int id);
        Task<R_Data> GetAsync(int provinceId, int districtId, int wardId);
        Task<R_Data> GetListAsync(Expression<Func<Address, bool>> expression);
        Task<R_Data> Delete(int id, int? updatedBy);
        Task<R_Data> PutAsync( string addresstext, int provinceid, int districtid, int wardid);
        Task<R_Data> PutAsync(int id, string addresstext,  int provinceid, int districtid, int wardid, DateTime? timer);
        Task<R_Data> PutAsync(int id, int? status, int? updatedBy, DateTime timer);
    }
    public class AddressService: IAddressService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AddressService> _logger;
        public AddressService(IUnitOfWork unitOfWork, ILogger<AddressService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<R_Data> Delete(int id, int? updatedBy)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<Address>(new Address());
            try
            {
                Expression<Func<Address, bool>> filter;
                filter = w => w.Id == id;
                categoryObj = _unitOfWork.AddressRepository.Find(filter);
                if(categoryObj==null)
                {
                    res.result = 0;
                    errObj.message = $"Không tìm thấy {id} để xóa.";
                }
                else
                {
                    //categoryObj.UpdatedBy = categoryObj.UpdatedBy;
                    _unitOfWork.AddressRepository.Delete(categoryObj);
                    var result = await _unitOfWork.CommitAsync();
                    if(result>0)
                    {
                        categoryObj = await _unitOfWork.AddressRepository.GetId(id);
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
            var categoryObj = await Task.FromResult<Address>(new Address());
            try
            {
                categoryObj = await _unitOfWork.AddressRepository.GetId(id);
                if(categoryObj==null)
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
        public async Task<R_Data> GetAsync(int provinceId, int districtId, int wardId)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<Address>(new Address());
            try
            {
                Expression<Func<Address, bool>> filter;
                filter = w => w.Status == 1 && w.ProvinceId==provinceId && w.DistrictId==districtId && w.WardId==wardId;
                filter.Compile();
                categoryObj = _unitOfWork.AddressRepository.Find(filter);
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
        public async Task<R_Data> GetListAsync(Expression<Func<Address, bool>> expression)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var lstObj = await Task.FromResult<List<Address>>(new List<Address>());
            try
            {
                lstObj = (await _unitOfWork.AddressRepository.ListAsync(expression)).ToList();
                if(lstObj==null)
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

        public async Task<R_Data> PutAsync( string addresstext, int provinceid, int districtid, int wardid) 
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<Address>(new Address());
            var idMax = await _unitOfWork.AddressRepository.Max();
            if (idMax.code != 1)
                throw new Exception();

            Address item = new Address()
            {
                Id = idMax.data + 1,
                Title = null,
                AddressNumber = null,
                AddressText = addresstext,
                CountryId = 1,
                ProvinceId = provinceid,
                DistrictId = districtid,
                WardId = wardid,
                CreatedAt= DateTime.Now,
                UpdatedAt=DateTime.Now,
                Timer = DateTime.Now,
                Status = 1
            };
            try
            {
                _unitOfWork.AddressRepository.Add(item);
                var result = await _unitOfWork.CommitAsync();
                if(result>0)
                {
                    categoryObj = await _unitOfWork.AddressRepository.GetId(item.Id);
                    errObj.message = "Thêm dữ liệu thành công.";
                }
                res.data = categoryObj;
            }
            catch (Exception ex)
            {
                res.result = 0;
                res.data = null;
                res.error = new error { code = 201, message = $"Exception: Xẩy ra lỗi khi thêm dữ liệu {ex}" };
            }


            return res;
        }

        public async Task<R_Data> PutAsync(int id, string addresstext, int provinceid, int districtid, int wardid, DateTime? timer)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<Address>(new Address());
            var existAddress = await _unitOfWork.AddressRepository.GetId(id);
            if(existAddress==null)
            {
                throw new Exception($"Grade {id} không tìm thấy.");
            }
            if(existAddress.Timer>timer)
            {
                res.result = 0;
                res.data = null;
                res.error = new error() { code = 201, message = "Thông tin đã được cập nhật lại trước đó. Vui lòng hủy thao tác và thực hiện lại để dữ liệu đồng bộ!" };
                return res;
            }
            Address item = new Address()
            {
                Id=id,
                Title = existAddress.Title,
                AddressNumber = existAddress.AddressNumber,
                AddressText = addresstext,
                CountryId = 1,
                ProvinceId = provinceid,
                DistrictId = districtid,
                WardId = wardid,
                Latitude=existAddress.Latitude,
                Longitude=existAddress.Longitude,
                Status=existAddress.Status,
                CreatedAt=existAddress.CreatedAt,
                CreatedBy=existAddress.CreatedBy,
                UpdatedBy=existAddress.UpdatedBy,
                Timer=existAddress.Timer,
                UpdatedAt = DateTime.Now,
                
            };
            try
            {
                await _unitOfWork.AddressRepository.UpdateAsync(item);
                var result = await _unitOfWork.CommitAsync();
                if(result>0)
                {
                    categoryObj = await _unitOfWork.AddressRepository.GetId(item.Id);
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


            return await Task.Run(()=>res);
        }
        public async Task<R_Data> PutAsync(int id, int? status, int? updatedBy, DateTime timer)
        {
            error errObj = new error();
            R_Data res = new R_Data() { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<Address>(new Address());


            var existingAddress = await _unitOfWork.AddressRepository.GetId(id);
            //var existingPerson = new InternshipContext().Persons.FirstOrDefault(f => f.Id == id);
            if (existingAddress == null)
                throw new Exception($"Person {id} không tìm thấy.");

            if (existingAddress.Timer > timer)
            {
                res.result = 0;
                res.data = null;
                res.error = new error() { code = 201, message = "Thông tin đã được cập nhật lại trước đó. Vui lòng hủy thao tác và thực hiện lại để dữ liệu đồng bộ!" };
                return res;
            }

            Address item = new Address
            {
                Id = existingAddress.Id,
                Title=existingAddress.Title,
                AddressNumber= existingAddress.AddressNumber,
                AddressText= existingAddress.AddressText,
                CountryId=existingAddress.CountryId,
                ProvinceId=existingAddress.ProvinceId,
                DistrictId=existingAddress.DistrictId,
                WardId=existingAddress.WardId,
                Latitude=existingAddress.Latitude,
                Longitude=existingAddress.Longitude,
                CreatedAt = existingAddress.CreatedAt,
                CreatedBy = existingAddress.CreatedBy,
                UpdatedAt = DateTime.Now,
                UpdatedBy = updatedBy,
                Status = status,
                Timer = DateTime.Now,
            };

            try
            {
                await _unitOfWork.AddressRepository.UpdateAsync(item);
                //await _unitOfWork.PersonRepository.UpdateAsync(item);
                var result = await _unitOfWork.CommitAsync();
                if (result > 0)
                {
                    categoryObj = await _unitOfWork.AddressRepository.GetId(item.Id);
                    errObj.message = "Cập nhật dữ liệu thành công.";
                }
                res.data = categoryObj;
            }
            catch (Exception ex)
            {
                res.result = 0;
                res.data = null;
                res.error = new error { code = 201, message = $"Exception: Xẩy ra lỗi khi thêm dữ liệu {ex}" };

            }
            return res;
        }
    }
}
