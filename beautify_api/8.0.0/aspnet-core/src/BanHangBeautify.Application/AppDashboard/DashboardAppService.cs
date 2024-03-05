using Abp.Authorization;
using BanHangBeautify.AppDashboard.Dto;
using BanHangBeautify.AppDashboard.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDashboard
{
    [AbpAuthorize]
    public class DashboardAppService : SPAAppServiceBase
    {
        private readonly IDashboardRepository _dashboardRepository;
        public DashboardAppService(
              IDashboardRepository dashboardRepository
              )
        {
            _dashboardRepository = dashboardRepository;
        }


        public async Task<ThongKeSoLuong> ThongKeSoLuong(DashboardFilterDto input)
        {
            return await _dashboardRepository.ThongKeThongTin(input, AbpSession.TenantId ?? 1, AbpSession.UserId);
        }
        public async Task<List<DanhSachLichHen>> DanhSachLichHen(DashboardFilterDto input)
        {
            DateTime now = DateTime.Now;
            input.ThoiGianTu = new DateTime(now.Year, now.Month, now.Day, 0, 0, 1).ToString("yyyy-MM-dd HH:mm:ss");
            input.ThoiGianDen = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59).ToString("yyyy-MM-dd HH:mm:ss");
            return await _dashboardRepository.DanhSachLichHen(input, AbpSession.TenantId ?? 1, AbpSession.UserId);
        }
        public async Task<List<ThongKeLichHen>> ThongKeLichHen(DashboardFilterDto input)
        {
            return await _dashboardRepository.ThongKeLichHen(input.IdChiNhanh, AbpSession.TenantId ?? 1);
        }
        public async Task<List<ThongKeDoanhThu>> ThongKeDoanhThu(DashboardFilterDto input)
        {
            return await _dashboardRepository.ThongKeDoanhThu(input.IdChiNhanh, AbpSession.TenantId ?? 1);
        }
        public async Task<List<HotService>> ThongKeHotService(DashboardFilterDto input)
        {
            return await _dashboardRepository.DanhSachDichVuHot(input, AbpSession.TenantId ?? 1, AbpSession.UserId);
        }
    }
}
