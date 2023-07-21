using Abp.Dependency;
using Abp.EntityFrameworkCore;
using BanHangBeautify.AppDashboard.Dto;
using BanHangBeautify.Entities;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDashboard.Repository
{
    public class DashboardRepository : SPARepositoryBase, IDashboardRepository
    {
        public DashboardRepository(IDbContextProvider<SPADbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<DataSet> ThongKeThongTin(DashboardFilterDto input, int tenantId, long? userId)
        {
            using (var command = CreateCommand("prc_ThongKeThongTin"))
            {
                command.Parameters.Add(new SqlParameter("@UserId", userId));
                command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
                command.Parameters.Add(new SqlParameter("@IdChiNhanh", input.IdChiNhanh));
                command.Parameters.Add(new SqlParameter("@ThoiGianTu", input.ThoiGianTu));
                command.Parameters.Add(new SqlParameter("@ThoiGianDen", input.ThoiGianDen));
                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    string[] array = { "DataTotal", "HotService", "Appointment" };
                    var ds = new DataSet();
                    ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                    return ds;
                }
            }
        }
    }
}
