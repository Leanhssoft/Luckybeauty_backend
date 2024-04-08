using BanHangBeautify.SMS.Dto;

namespace BanHangBeautify.ZaloSMS_Common
{
    public interface ICommonZaloSMS
    {
        string ReplaceContent_Withkey(PageKhachHangSMSDto cutomer, string key);
        string ReplaceContent(PageKhachHangSMSDto cutomer, string noiDungTin);
    }
}
