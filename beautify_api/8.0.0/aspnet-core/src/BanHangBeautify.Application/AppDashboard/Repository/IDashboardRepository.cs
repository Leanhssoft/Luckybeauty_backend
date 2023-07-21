using BanHangBeautify.AppDashboard.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDashboard.Repository
{
    public interface IDashboardRepository
    {
        Task<DataSet> ThongKeThongTin(DashboardFilterDto input, int tenantId, long? userId);
    }
}
