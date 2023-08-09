using Abp.Authorization;
using BanHangBeautify.AppDashboard.Dto;
using BanHangBeautify.AppDashboard.Repository;
using System.Collections.Generic;
using System.Data;
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
