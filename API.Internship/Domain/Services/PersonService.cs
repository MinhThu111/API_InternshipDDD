using API.Internship.Domain.Interfaces;
using API.Internship.Domain.Models;
using API.Internship.ResData;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Linq.Expressions;
using System.Xml.Linq;

namespace API.Internship.Domain.Services
{
    public interface IPersonService
    {
        Task<R_Data> GetAsync(int id);
        Task<R_Data> GetListAsync(Expression<Func<Person, bool>> expression);
        Task<R_Data> GetListAsync();
        //Task<R_Data> GetListAsync(string status);
        Task<R_Data> Delete(int id, int? updatedBy);
        Task<R_Data> PutAsync(int id, string firstname, string lastname, int? gender, int persontypeid, DateTime timer, int? status, int? addressid, string phonenumber, string email);
        Task<R_Data> PutAsync(string fsname, string lsname, int persontypeid, DateTime? birthday, int? gender, int? nationalityid, int? regilionid, int? folkid, int? addressid, string phone, string email, string avatarurl);
        Task<R_Data> PutAsync(int id, int? status, int? updatedBy, DateTime timer);
    }
    public class PersonService: IPersonService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PersonService> _logger;
        public PersonService(IUnitOfWork unitOfWork, ILogger<PersonService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<R_Data> GetAsync(int id)
        {
            error errObj = new error();
            R_Data res = new R_Data() { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<Person>(new Person());
            try
            {
                categoryObj = await _unitOfWork.PersonRepository.GetId(id);
                if (categoryObj == null)
                    errObj.message = "Load data is successful and do not data to show!";
                else
                    res.data = categoryObj;
            }
            catch (Exception ex)
            {
                res.result = 0;
                res.data = null;
                res.error = new error { code = 201, message = $"Exception: Xẩy ra lỗi khi đọc dữ liệu {ex}" };
            }
            return res;
        }
        public async Task<R_Data> GetListAsync(Expression<Func<Person, bool>> expression)
        {
            error errObj = new error();
            R_Data res = new R_Data() { result = 1, data = null, error = errObj };
            var lstPersons = await Task.FromResult<List<Person>>(new List<Person>());
            try
            {
                lstPersons = (await _unitOfWork.PersonRepository.ListAsync(expression)).ToList();
                if (lstPersons.Count <= 0)
                    errObj.message = "Load data is successful and do not data to show!";
                else
                    res.data = lstPersons;
            }
            catch (Exception ex)
            {
                res.result = 0;
                res.data = null;
                res.error = new error { code = 201, message = $"Exception: Xẩy ra lỗi khi đọc dữ liệu {ex}" };
            }
            return res;

        }
        public async Task<R_Data> GetListAsync()
        {
            error errObj = new error();
            R_Data res = new R_Data() { result = 1, data = null, error = errObj };
            var lstPersons = await Task.FromResult<List<Person>>(new List<Person>());
            try
            {
                Expression<Func<Person, bool>> filter;
                filter = w => w.Status == 1;
                filter.Compile();
                lstPersons = (await _unitOfWork.PersonRepository.ListAsync(filter)).ToList();

                var datacount = from item in lstPersons
                                group item by item.PersonTypeId into lstitem
                                select new
                                {
                                    name = lstitem.Key==1?"student":"teacher",
                                    count = lstitem.Count(),
                                };

                if (datacount.Count()   <= 0)
                    errObj.message = "Load data is successful and do not data to show!";
                else
                    res.data = datacount;
            }
            catch (Exception ex)
            {
                res.result = 0;
                res.data = null;
                res.error = new error { code = 201, message = $"Exception: Xẩy ra lỗi khi đọc dữ liệu {ex}" };
            }
            return res;

        }
        public async Task<R_Data> Delete(int id, int? updatedBy)
        {
            error errObj = new error();
            R_Data res = new R_Data() { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<Person>(new Person());
            try
            {
                Expression<Func<Person, bool>> filter;
                filter = a => a.Id == id;
                categoryObj = _unitOfWork.PersonRepository.Find(filter); if (categoryObj == null)
                {
                    res.result = 0;
                    errObj.message = $"Không tìm thấy {id} để xóa.";
                }
                else
                {
                    categoryObj.UpdatedBy = categoryObj.UpdatedBy;
                    _unitOfWork.PersonRepository.Delete(categoryObj);
                    var result = await _unitOfWork.CommitAsync();
                    if (result > 0)
                    {
                        categoryObj = await _unitOfWork.PersonRepository.GetId(id);
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
        public async Task<R_Data> PutAsync(int id, string firstname, string lastname, int? gender, int persontypeid, DateTime timer, int? status, int? addressid, string phonenumber, string email)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<Person>(new Person());

            var existingPerson = await _unitOfWork.PersonRepository.GetId(id);
            if(existingPerson == null)
            {
                throw new Exception($"Person {id} không tìm thấy.");
            }
            if (existingPerson.Timer > timer)
            {
                res.result = 0;
                res.data = null;
                res.error = new error() { code = 201, message = "Thông tin đã được cập nhật lại trước đó. Vui lòng hủy thao tác và thực hiện lại để dữ liệu đồng bộ!" };
                return res;
            }
            Person item = new Person()
            {
                Id=existingPerson.Id,
                FirstName = firstname,
                LastName = lastname,
                Gender = gender,
                PersonTypeId = persontypeid,
                Status = status,
                AddressId = addressid,
                PhoneNumber = phonenumber,
                Birthday = existingPerson.Birthday,
                NationalityId = existingPerson.NationalityId,
                ReligionId = existingPerson.ReligionId,
                FolkId = existingPerson.FolkId,
                Email = email,
                Timer = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            try
            {
                await _unitOfWork.PersonRepository.UpdateAsync(item);
                var result = await _unitOfWork.CommitAsync();
                if (result > 0)
                {
                    categoryObj = await _unitOfWork.PersonRepository.GetId(item.Id);
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
        public async Task<R_Data> PutAsync(string fsname, string lsname, int persontypeid, DateTime? birthday, int? gender, int? nationalityid, int? regilionid, int? folkid, int? addressid, string phone, string email, string avatarurl)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<Person>(new Person());
            var idMax = await _unitOfWork.PersonRepository.Max();
            if (idMax.code != 1)
                throw new Exception();
            Person item = new Person()
            {
                Id = idMax.data + 1,
                Status = 1,
                FirstName = fsname,
                LastName = lsname,
                PersonTypeId = persontypeid,
                Birthday = birthday,
                Gender = gender,
                NationalityId = nationalityid,
                ReligionId = regilionid,
                FolkId = folkid,
                AddressId = addressid,
                PhoneNumber = phone,
                Email = email,
                AvatarUrl=avatarurl,
                Timer = DateTime.Now,
                CreatedAt = DateTime.Now
            };
            try
            {
                _unitOfWork.PersonRepository.Add(item);
                var result = await _unitOfWork.CommitAsync();
                if (result > 0)
                {
                    categoryObj = await _unitOfWork.PersonRepository.GetId(item.Id);
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
        public async Task<R_Data> PutAsync(int id, int? status, int? updatedBy, DateTime timer)
        {
            error errObj = new error();
            R_Data res = new R_Data() { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<Person>(new Person());


            var existingPerson = await _unitOfWork.PersonRepository.GetId(id);
            //var existingPerson = new InternshipContext().Persons.FirstOrDefault(f => f.Id == id);
            if (existingPerson == null)
                throw new Exception($"Person {id} không tìm thấy.");

            if (existingPerson.Timer > timer)
            {
                res.result = 0;
                res.data = null;
                res.error = new error() { code = 201, message = "Thông tin đã được cập nhật lại trước đó. Vui lòng hủy thao tác và thực hiện lại để dữ liệu đồng bộ!" };
                return res;
            }

            Person item = new Person
            {
                Id = existingPerson.Id,
                FirstName = existingPerson.FirstName,
                LastName=existingPerson.LastName,
                Gender=existingPerson.Gender,
                PersonTypeId=existingPerson.PersonTypeId,
                AddressId=existingPerson.AddressId,
                Birthday=existingPerson.Birthday,
                ReligionId=existingPerson.ReligionId,
                FolkId=existingPerson.FolkId,
                NationalityId=existingPerson.NationalityId,
                PhoneNumber=existingPerson.PhoneNumber,
                Email=existingPerson.Email,
                Code=existingPerson.Code,
                Remark = existingPerson.Remark,
                CreatedAt = existingPerson.CreatedAt,
                CreatedBy = existingPerson.CreatedBy,
                UpdatedAt = DateTime.Now,
                UpdatedBy = updatedBy,
                Status = status,
                Timer = DateTime.Now,
            };

            try
            {
                await _unitOfWork.PersonRepository.UpdateAsync(item);
                //await _unitOfWork.PersonRepository.UpdateAsync(item);
                var result = await _unitOfWork.CommitAsync();
                if (result > 0)
                {
                    categoryObj = await _unitOfWork.PersonRepository.GetId(item.Id);
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
