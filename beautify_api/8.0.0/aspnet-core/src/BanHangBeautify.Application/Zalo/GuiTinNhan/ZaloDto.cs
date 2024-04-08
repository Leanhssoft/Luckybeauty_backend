using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Zalo.GuiTinNhan
{
    public class ZaloDataDto
    {
        public string message_id { get; set; }
        public string user_id { get; set; }
    }

    public class ResultMessageZaloDto
    {
        public ZaloDataDto data { get; set; } // ID của tin nhắn mới được tạo 
        public int error { get; set; }// = 0, Gửi thành công
        public string message { get; set; }// nếu gửi thành công message = Success, ngược lại: thông báo lỗi
    }

    public class Zalo_InforHoaDonSend
    {
        public Guid? Id { get; set; }
        public Guid? IdKhachHang { get; set; }
        public string MaHoaDon { get; set; }
        public DateTime? NgayLapHoaDon { get; set; }
        public double? TongTienHang { get; set; }
    }

    // Khai báo các lớp để mô tả kiểu dữ liệu trong dữ liệu JSON khi gửi tin nhắn Zalo
    public class ZaloRecipient
    {
        public string user_id { get; set; }
    }

    public class ZaloPayloadContent
    {
        public string value { get; set; }
        public string key { get; set; }
    }

    public class ZaloButton
    {
        public string title { get; set; }
        public string image_icon { get; set; }
        public string type { get; set; }
        public object payload { get; set; }
    }

    public class ZaloPayload
    {
        public string template_type { get; set; }
        public string language { get; set; }
        public List<object> elements { get; set; }
        public List<ZaloButton> buttons { get; set; }
    }

    public class ZaloAttachment
    {
        public string type { get; set; }
        public ZaloPayload payload { get; set; }
    }

    public class ZaloMessage
    {
        public string text { get; set; }
        public ZaloAttachment attachment { get; set; }
    }

    public class ZaloRequestData
    {
        public ZaloRecipient recipient { get; set; }
        public ZaloMessage message { get; set; }
    }
}
