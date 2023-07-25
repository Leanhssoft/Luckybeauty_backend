using BanHangBeautify.AppDashboard.Dto;
using System.Data;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDashboard.Repository
{
    public interface IDashboardRepository
    {
        Task<DataSet> ThongKeThongTin(DashboardFilterDto input, int tenantId, long? userId);
    }
}
