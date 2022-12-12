namespace API.Internship.ResData
{
    public class R_Data
    {
        public int result { get; set; } //0 | 1
        public dynamic data { get; set; } = null;
        public error error { get; set; }

    }
    public class error
    {
        public int code { get; set; }
        public string message { get; set; }
        public error()
        {
            code = 200;
            message = string.Empty;
        }
        public error(int _code, string _messege)
        {
            code = _code;
            message = _messege;
        }
    }

    public class internalData
    {

        public int code { get; set; } //Chỉ mang 3 trạng thái: -1: Exception; 0: không tồn tại hoặc không có xử lý dữ liệu; 1: Có tồn tại hoặc có xử lý dữ liệu
        public string message { get; set; }
        public dynamic data { get; set; }
        public internalData()
        {
            code = 1;
            message = string.Empty;
            data = null;
        }

    }
}
