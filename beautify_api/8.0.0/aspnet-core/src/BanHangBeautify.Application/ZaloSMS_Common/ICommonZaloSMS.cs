using BanHangBeautify.AppDanhMuc.AppCuaHang.Dto;
using BanHangBeautify.SMS.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.ZaloSMS_Common
{
    public interface ICommonZaloSMS
    {
        string ReplaceContent_Withkey(PageKhachHangSMSDto cutomer, string key);
        string ReplaceContent(PageKhachHangSMSDto cutomer, string noiDungTin);
    }
}
