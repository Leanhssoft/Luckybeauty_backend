using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.SMS.Dto
{
    public class ZaloDto
    {
        public string AOID { get; set; }
        public string TempID { get; set; } // Template của Zalo OA mà khách hàng đăng kí với eSMS --> Id của mẫu tin Zalo
        public List<string> Params { get; set; }// * Giá trị cần truyền cho các biến trong Template theo thứ tự
        public string Phone { get; set; }// * Số điện thoại nhận tin
        public string OAID { get; set; }// * Id Zalo Offical Account của doanh nghiệp
        public string ApiKey { get; set; }
        public string SecretKey { get; set; }
        public string SendDate { get; set; }// Thời gian hẹn gửi của tin yyyy-mm-dd hh:MM:ss
    }

    public class ZaloDataDto
    {
        [JsonProperty("message_id")]
        public string MessageId { get; set; }
        [JsonProperty("user_id")]
        public string UserId { get; set; }
    }

    public class ResultZaloDto
    {
        [JsonProperty("data")]
        public ZaloDataDto Data { get; set; } // ID của tin nhắn mới được tạo 
        [JsonProperty("data")]
        public string Error { get; set; }// = 0, Gửi thành công
        [JsonProperty("message")]
        public string Message { get; set; }// nếu gửi thành công message = Success, ngược lại: thông báo lỗi
    }

    public class Zalo_InforHoaDonSend
    {
        public Guid? Id { get; set; }
        public Guid? IdKhachHang { get; set; }
        public string MaHoaDon { get; set; }
        public DateTime? NgayLapHoaDon { get; set; }
        public double? TongTienHang { get; set; }
    }
}
