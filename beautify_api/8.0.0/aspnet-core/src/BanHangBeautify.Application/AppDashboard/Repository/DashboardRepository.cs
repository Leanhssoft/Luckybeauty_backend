using Abp.Application.Services.Dto;
using Abp.EntityFrameworkCore;
using BanHangBeautify.AppCommon;
using BanHangBeautify.AppDashboard.Dto;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using BanHangBeautify.HoaDon.HoaDon.Dto;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;


namespace BanHangBeautify.AppDashboard.Repository
{
    public class DashboardRepository : SPARepositoryBase, IDashboardRepository
    {
        public DashboardRepository(IDbContextProvider<SPADbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<ThongKeSoLuong> ThongKeThongTin(CommonClass.ParamSearch input)
        {
            string idChiNhanhs= string.Empty;
            if(input.IdChiNhanhs!=null && input.IdChiNhanhs.Count > 0)
            {
                idChiNhanhs = string.Join(",", input.IdChiNhanhs);
            }
            using (var command = CreateCommand("prc_dashboard_thongKeSoLuong"))
            {
                command.Parameters.Add(new SqlParameter("@TenantId", input?.TenantId));
                command.Parameters.Add(new SqlParameter("@ThoiGianTu", input?.FromDate ?? DateTime.Now));
                command.Parameters.Add(new SqlParameter("@ThoiGianDen", input?.ToDate ?? DateTime.Now));
                command.Parameters.Add(new SqlParameter("@IdChiNhanhs", idChiNhanhs));
                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    string[] array = { "Data" };
                    var ds = new DataSet();
                    ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                    var ddd = ds.Tables;

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var data = ObjectHelper.FillObject<ThongKeSoLuong>(ds.Tables[0].Rows[0]);
                        return data;
                    }
                }
                return new ThongKeSoLuong();
            }
        }
        public async Task<PagedResultDto<DanhSachLichHen>> DanhSachLichHen(CommonClass.ParamSearch input)
        {
            string idChiNhanhs = string.Empty;
            if (input.IdChiNhanhs != null && input.IdChiNhanhs.Count > 0)
            {
                idChiNhanhs = string.Join(",", input.IdChiNhanhs);
            }
            using (var command = CreateCommand("prc_dashboard_danhSachLichHen"))
            {
                command.Parameters.Add(new SqlParameter("@UserId", input.IdUserLogin));
                command.Parameters.Add(new SqlParameter("@TenantId", input?.TenantId));
                command.Parameters.Add(new SqlParameter("@IdChiNhanhs", idChiNhanhs));
                command.Parameters.Add(new SqlParameter("@ThoiGianTu", input?.FromDate?? DateTime.Now));
                command.Parameters.Add(new SqlParameter("@ThoiGianDen", input?.ToDate ?? DateTime.Now));
                command.Parameters.Add(new SqlParameter("@CurrentPage", input?.CurrentPage ?? 1));
                command.Parameters.Add(new SqlParameter("@PageSize", input?.PageSize ?? 3));// dashborad: get top 3
                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    string[] array = { "Data" };
                    var ds = new DataSet();
                    ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                    var ddd = ds.Tables;

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var data = ObjectHelper.FillCollection<DanhSachLichHen>(ds.Tables[0]);
                        return new PagedResultDto<DanhSachLichHen>()
                        {
                            TotalCount = Int32.Parse(ds.Tables[0].Rows[0]["TotalRow"].ToString()),
                            Items = data
                        };
                    }
                }
                return new PagedResultDto<DanhSachLichHen>();
            }
        }
        public async Task<List<ThongKeLichHen>> ThongKeLichHen(CommonClass.ParamSearch input)
        {
            string idChiNhanhs = string.Empty;
            if (input.IdChiNhanhs != null && input.IdChiNhanhs.Count > 0)
            {
                idChiNhanhs = string.Join(",", input.IdChiNhanhs);
            }
            using (var command = CreateCommand("prc_dashboard_thongKeLichHen"))
            {
                command.Parameters.Add(new SqlParameter("@TenantId", input?.TenantId));
                command.Parameters.Add(new SqlParameter("@IdChiNhanhs", idChiNhanhs));
                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    string[] array = { "Data" };
                    var ds = new DataSet();
                    ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                    var ddd = ds.Tables;

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var data = ObjectHelper.FillCollection<ThongKeLichHen>(ds.Tables[0]);
                        return data;
                    }
                }
                return new List<ThongKeLichHen>();
            }
        }
        public async Task<List<ThongKeDoanhThu>> ThongKeDoanhThu(CommonClass.ParamSearch input)
        {
            string idChiNhanhs = string.Empty;
            if (input.IdChiNhanhs != null && input.IdChiNhanhs.Count > 0)
            {
                idChiNhanhs = string.Join(",", input.IdChiNhanhs);
            }
            using (var command = CreateCommand("prc_dashboard_thongKeDoanhThu"))
            {
                command.Parameters.Add(new SqlParameter("@TenantId", input?.TenantId));
                command.Parameters.Add(new SqlParameter("@IdChiNhanhs", idChiNhanhs));
                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    string[] array = { "Data" };
                    var ds = new DataSet();
                    ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                    var ddd = ds.Tables;

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var data = ObjectHelper.FillCollection<ThongKeDoanhThu>(ds.Tables[0]);
                        return data;
                    }
                }
                return new List<ThongKeDoanhThu>();
            }
        }
        public async Task<List<HotService>> DanhSachDichVuHot(CommonClass.ParamSearch input)
        {
            string idChiNhanhs = string.Empty, loaiBaoCao =  "0";
            if (input.IdChiNhanhs != null && input.IdChiNhanhs.Count > 0)
            {
                idChiNhanhs = string.Join(",", input.IdChiNhanhs);
            } 
            if (input.IdChiNhanhs != null && input.IdChiNhanhs.Count > 0)
            {
                // mượn trường TrangThai: 0.top dich vụ theo Doanh thu, 1.theo số lượng
                loaiBaoCao = input.TrangThais[0];
            }
            using (var command = CreateCommand("prc_dashboard_hotService"))
            {
                command.Parameters.Add(new SqlParameter("@TenantId", input?.TenantId));
                command.Parameters.Add(new SqlParameter("@LoaiBaoCao", loaiBaoCao));
                command.Parameters.Add(new SqlParameter("@IdChiNhanhs", idChiNhanhs));
                command.Parameters.Add(new SqlParameter("@ThoiGianTu", input?.FromDate ?? DateTime.Now));
                command.Parameters.Add(new SqlParameter("@ThoiGianDen", input?.ToDate ?? DateTime.Now));
                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    string[] array = { "Data" };
                    var ds = new DataSet();
                    ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                    var ddd = ds.Tables;

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var data = ObjectHelper.FillCollection<HotService>(ds.Tables[0]);
                        return data;
                    }
                }
                return new List<HotService>();
            }
        }
    }
}
