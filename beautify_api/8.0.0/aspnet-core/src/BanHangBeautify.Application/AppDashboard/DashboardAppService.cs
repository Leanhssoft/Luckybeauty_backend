using Abp.Authorization;
using BanHangBeautify.AppDashboard.Dto;
using BanHangBeautify.AppDashboard.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BanHangBeautify.AppCommon;
using Abp.Application.Services.Dto;

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


        public async Task<ThongKeSoLuong> ThongKeSoLuong(CommonClass.ParamSearch input)
        {
            input.TenantId = AbpSession.TenantId ?? 1;
            input.IdUserLogin = AbpSession.UserId;
            return await _dashboardRepository.ThongKeThongTin(input);
        }
        public async Task<PagedResultDto<DanhSachLichHen>> DanhSachLichHen(CommonClass.ParamSearch input)
        {
            input.TenantId = AbpSession.TenantId ?? 1;
            input.IdUserLogin = AbpSession.UserId;
            return await _dashboardRepository.DanhSachLichHen(input);
        }
        public async Task<List<ThongKeLichHen>> ThongKeLichHen(CommonClass.ParamSearch input)
        {
            input.TenantId = AbpSession.TenantId ?? 1;
            input.IdUserLogin = AbpSession.UserId;
            return await _dashboardRepository.ThongKeLichHen(input);
        }
        public async Task<List<ThongKeDoanhThu>> ThongKeDoanhThu(CommonClass.ParamSearch input)
        {
            input.TenantId = AbpSession.TenantId ?? 1;
            return await _dashboardRepository.ThongKeDoanhThu(input);
        }
        public async Task<List<HotService>> ThongKeHotService(CommonClass.ParamSearch input)
        {
            input.TenantId = AbpSession.TenantId ?? 1;
            return await _dashboardRepository.DanhSachDichVuHot(input);
        }
    }
}
