using BanHangBeautify.AppDashboard.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDashboard.Repository
{
    public interface IDashboardRepository
    {
        Task<ThongKeSoLuong> ThongKeThongTin(DashboardFilterDto input, int tenantId, long? userId);
        Task<List<ThongKeLichHen>> ThongKeLichHen(Guid idChiNhanh, int tenantId);
        Task<List<ThongKeDoanhThu>> ThongKeDoanhThu(Guid idChiNhanh, int tenantId);
        Task<List<HotService>> DanhSachDichVuHot(DashboardFilterDto input, int tenantId, long? userId);
        Task<List<DanhSachLichHen>> DanhSachLichHen(DashboardFilterDto input, int tenantId, long? userId);
    }
}
