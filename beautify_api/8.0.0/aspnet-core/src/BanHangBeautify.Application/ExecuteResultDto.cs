namespace BanHangBeautify
{
    public class ExecuteResultDto
    {
        public string Status { get; set; } = "error";

        public string Message { get; set; } = "Có lỗi xảy ra vui lòng thử lại sau!";

        public string Detail { get; set; }
    }
}
