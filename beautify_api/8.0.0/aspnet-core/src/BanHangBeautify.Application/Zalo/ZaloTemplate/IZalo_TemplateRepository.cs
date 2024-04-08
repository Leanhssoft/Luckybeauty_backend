using BanHangBeautify.SMS.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Zalo.ZaloTemplate
{
    public interface IZalo_TemplateRepository
    {
        List<Zalo_TemplateDto> InnitData_TempZalo();
        Task<Zalo_TemplateDto> FindTempDefault_ByIdLoaiTin(byte idLoaiTin);
        Zalo_TemplateDto GetZaloTemplate_byId(Guid id);
    }
}
