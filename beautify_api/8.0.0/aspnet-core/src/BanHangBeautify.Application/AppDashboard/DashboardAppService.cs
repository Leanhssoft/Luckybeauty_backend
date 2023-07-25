using Abp.Authorization;
using BanHangBeautify.AppDashboard.Dto;
using BanHangBeautify.AppDashboard.Repository;
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
        public async Task<DataSet> ThongKeThongTin(DashboardFilterDto input)
        {
            return await _dashboardRepository.ThongKeThongTin(input, AbpSession.TenantId ?? 1, AbpSession.UserId);
        }
    }
}
