using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.SMS.Dto
{
    public class ESMSDto
    {
        // giá trị các tham số này là do ESMS cung cấp
        public string Phone { get; set; }// sdt người nhận
        public string Content { get; set; }// nội dung tin nhắn
        public string Brandname { get; set; }
        //public int SmsType { get; set; }  //2. CSKH, 24.Zalo ưu tiên, 25. Zalo thường
        //public DateTime? SendDate { get; set; } = null;// đặt lịch gửi tin (ngày..): hiện tại chưa dùng
        //public string RequestId { get; set; }// ID Tin nhắn của đối tác, dùng để kiểm tra ID này đã được hệ thống esms tiếp nhận trước đó hay chưa (hiện tại chưa dùng)
        //public int? Sandbox { get; set; }// 1.Gửi thật, 1.Thử nghiệm (không gửi, mà chỉ tạo tin nhắn)
    }
    public class ResultSMSDto
    {
        public string MessageId { get; set; }
        public int MessageStatus { get; set; }// 100. thanh cong
    }
}
