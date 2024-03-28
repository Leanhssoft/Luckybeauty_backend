using BanHangBeautify.SMS.Dto;
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
        Task<ResultMessageZaloDto> GuiTinGiaoDich_fromDataDB(PageKhachHangSMSDto dataSend, string accessToken, Zalo_TemplateDto tempItem);
    }
}
