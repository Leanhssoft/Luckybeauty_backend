using BanHangBeautify.AppDashboard.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BanHangBeautify.AppCommon;
using Abp.Application.Services.Dto;

namespace BanHangBeautify.AppDashboard.Repository
{
    public interface IDashboardRepository
    {
        Task<ThongKeSoLuong> ThongKeThongTin(CommonClass.ParamSearch input);
        Task<List<ThongKeLichHen>> ThongKeLichHen(CommonClass.ParamSearch input);
        Task<List<ThongKeDoanhThu>> ThongKeDoanhThu(CommonClass.ParamSearch input);
        Task<List<HotService>> DanhSachDichVuHot(CommonClass.ParamSearch input);
        Task<PagedResultDto<DanhSachLichHen>> DanhSachLichHen(CommonClass.ParamSearch input);
    }
}
