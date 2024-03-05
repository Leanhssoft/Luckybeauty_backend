using Abp.Application.Services.Dto;
using BanHangBeautify.SMS.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BanHangBeautify.AppCommon.CommonClass;

namespace BanHangBeautify.SMS.LichSuNap_ChuyenTien.Repository
{
    public interface ILichSuNap_ChuyenTienRepository
    {
        Task<PagedResultDto<PageNhatKyChuyenTienDto>> GetAllNhatKyChuyenTien(ParamSearch param);
    }
}
