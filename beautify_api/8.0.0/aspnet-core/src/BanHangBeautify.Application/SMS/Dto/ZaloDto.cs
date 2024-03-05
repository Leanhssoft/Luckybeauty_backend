﻿using NPOI.SS.Formula.Functions;
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

    public class ResultZaloDto
    {
        public string SMSID { get; set; } // ID của tin nhắn mới được tạo 
        public string CodeResult { get; set; }// 100. thanh cong
        public string Phone { get; set; }// gui thang cong ? tin
    }
}
