using BanHangBeautify.SMS.Dto;
using BanHangBeautify.Zalo.DangKyThanhVien;
using BanHangBeautify.Zalo.ZaloTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Zalo.GuiTinNhan
{
    public interface IZaloSendMes
    {
        Task<ZNSTempleteDetailDto> GetZNSTemplateDetails_byId(string accessToken, string znsTempId);
        Task<ResultMessageZaloDto> GuiTinZalo_UseZNS(PageKhachHangSMSDto dataSend, string accessToken, ZNSTempleteDetailDto znsTemp);
        Task<ResultMessageZaloDto> GuiTinTruyenThongorGiaoDich_fromDataDB(PageKhachHangSMSDto dataSend, string accessToken, Guid zaloTempId);
    }
}
