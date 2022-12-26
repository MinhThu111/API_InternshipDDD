using API.Internship.Domain.Interfaces;
using API.Internship.Domain.Models;
using API.Internship.ResData;
using System.Linq.Expressions;
namespace API.Internship.Domain.Services
{
    public interface IScoreService
    {
        Task<R_Data> GetAsync(int id);
        Task<R_Data> GetListAsync(Expression<Func<Score, bool>> expression);
        Task<R_Data> PutAsync(int id, int? score, DateTime timer);
        Task<R_Data> PutAsync(int? score, int? scoretypeid, int? subjectid, int studentid, string remark);
        Task<R_Data> Delete(int id, int? updatedBy);
    }
    public class ScoreService: IScoreService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ScoreService> _logger;
        private readonly string refName = nameof(Score);
        public ScoreService(IUnitOfWork unitOfWork, ILogger<ScoreService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<R_Data> GetAsync(int id)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<Score>(new Score());
            try
            {
                categoryObj = await _unitOfWork.ScoreRepository.GetId(id);
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
        public async Task<R_Data> GetListAsync(Expression<Func<Score, bool>> expression)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var lstObj = await Task.FromResult<List<Score>>(new List<Score>());
            try
            {
                lstObj = (await _unitOfWork.ScoreRepository.ListAsync(expression)).ToList();
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
            var categoryObj = await Task.FromResult<Score>(new Score());
            try
            {
                Expression<Func<Score, bool>> filter;
                filter = w => w.Id == id;
                categoryObj = _unitOfWork.ScoreRepository.Find(filter);
                if (categoryObj == null)
                {
                    res.result = 0;
                    errObj.message = $"Không tìm thấy {id} để xóa.";
                }
                else
                {
                    _unitOfWork.ScoreRepository.Delete(categoryObj);
                    var result = await _unitOfWork.CommitAsync();
                    if (result > 0)
                    {
                        categoryObj = await _unitOfWork.ScoreRepository.GetId(id);
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
        public async Task<R_Data> PutAsync(int id, int? score, DateTime timer)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<Score>(new Score());
            var existScore = await _unitOfWork.ScoreRepository.GetId(id);
            if (existScore == null)
            {
                throw new Exception($"Grade {id} không tìm thấy.");
            }
            if (existScore.Timer > timer)
            {
                res.result = 0;
                res.data = null;
                res.error = new error() { code = 201, message = "Thông tin đã được cập nhật lại trước đó. Vui lòng hủy thao tác và thực hiện lại để dữ liệu đồng bộ!" };
                return res;
            }
            Score item = new Score()
            {
                Score1 = score,
                UpdatedAt = DateTime.Now
            };
            try
            {
                await _unitOfWork.ScoreRepository.UpdateAsync(item);
                var result = await _unitOfWork.CommitAsync();
                if (result > 0)
                {
                    categoryObj = await _unitOfWork.ScoreRepository.GetId(item.Id);
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
        public async Task<R_Data> PutAsync(int? score, int? scoretypeid, int? subjectid, int studentid, string remark)
        {
            error errObj = new error();
            R_Data res = new R_Data { result = 1, data = null, error = errObj };
            var categoryObj = await Task.FromResult<Score>(new Score());
            var idMax = await _unitOfWork.ScoreRepository.Max();
            if (idMax.code != 1)
                throw new Exception();

            Score item = new Score()
            {
                Id = idMax.data + 1,
                Score1 = score,
                ScoreTypeId = scoretypeid,
                SubjectId = subjectid,
                StudentId = studentid,
                Remark = remark,
                Status = 1,
                CreatedAt = DateTime.Now,
                Timer = DateTime.Now

            };

            try
            {
                _unitOfWork.ScoreRepository.Add(item);
                var result = await _unitOfWork.CommitAsync();
                if (result > 0)
                {
                    categoryObj = await _unitOfWork.ScoreRepository.GetId(item.Id);
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
}
