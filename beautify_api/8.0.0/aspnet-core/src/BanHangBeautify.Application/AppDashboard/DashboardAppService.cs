using Abp.Runtime.Session;
using BanHangBeautify.AppDashboard.Dto;
using BanHangBeautify.AppDashboard.Repository;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDashboard
{
    public class DashboardAppService: SPAAppServiceBase
    {
        private readonly IDasboardRepository _dashboardRepository;
        public DashboardAppService(
              IDasboardRepository dashboardRepository
              )
        {
            _dashboardRepository = dashboardRepository;
        }
        public async Task<DataSet> ThongKeThongTin(DashboardFilterDto input)
        {
            return await _dashboardRepository.ThongKeThongTin(input, AbpSession.TenantId??1, AbpSession.UserId);
        }
    }
}
