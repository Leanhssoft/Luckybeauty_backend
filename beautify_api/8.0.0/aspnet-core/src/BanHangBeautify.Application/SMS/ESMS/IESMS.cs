using BanHangBeautify.KhachHang.KhachHang.Dto;
using BanHangBeautify.SMS.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BanHangBeautify.Configuration.Common.CommonClass;

namespace BanHangBeautify.SMS.ESMS
{
    public interface IESMS
    {
        Task<ResultSMSDto> SendSMS_Json(ESMSDto obj);
        Task<bool> SenEmail_ToListCustomer(ParamSearch input, byte? idLoaiTin, EmailDto objEmail);
        Task<bool> SendSMS_ToListCustomer(ParamSearch input, byte? idLoaiTin, EmailDto objEmail);
    }
}
