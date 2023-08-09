using Abp.EntityFrameworkCore;
using BanHangBeautify.AppDashboard.Dto;
using BanHangBeautify.Authorization.Users;
using BanHangBeautify.Common;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using BanHangBeautify.HangHoa.HangHoa.Dto;
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

        public async Task<ThongKeSoLuong> ThongKeThongTin(DashboardFilterDto input, int tenantId, long? userId)
        {
            using (var command = CreateCommand("prc_dashboard_thongKeSoLuong"))
            {
                command.Parameters.Add(new SqlParameter("@UserId", userId));
                command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
                command.Parameters.Add(new SqlParameter("@IdChiNhanh", input.IdChiNhanh));
                command.Parameters.Add(new SqlParameter("@ThoiGianTu", input.ThoiGianTu));
                command.Parameters.Add(new SqlParameter("@ThoiGianDen", input.ThoiGianDen));
                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    string[] array = { "Data" };
                    var ds = new DataSet();
                    ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                    var ddd = ds.Tables;

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var data = ObjectHelper.FillObject<ThongKeSoLuong>(ds.Tables[0].Rows[0]);
                        data.TongDoanhThu = decimal.Parse(string.IsNullOrEmpty(ds.Tables[0].Rows[0]["TongDoanhThu"].ToString()) ? "0": ds.Tables[0].Rows[0]["TongDoanhThu"].ToString());
                        return data;
                    }
                }
                return new ThongKeSoLuong();
            }
        }
        public async Task<List<DanhSachLichHen>> DanhSachLichHen(DashboardFilterDto input, int tenantId, long? userId)
        {
            using (var command = CreateCommand("prc_dashboard_danhSachLichHen"))
            {
                command.Parameters.Add(new SqlParameter("@UserId", userId));
                command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
                command.Parameters.Add(new SqlParameter("@IdChiNhanh", input.IdChiNhanh));
                command.Parameters.Add(new SqlParameter("@ThoiGianTu", input.ThoiGianTu));
                command.Parameters.Add(new SqlParameter("@ThoiGianDen", input.ThoiGianDen));
                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    string[] array = { "Data" };
                    var ds = new DataSet();
                    ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                    var ddd = ds.Tables;

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var data = ObjectHelper.FillCollection<DanhSachLichHen>(ds.Tables[0]);
                        return data;
                    }
                }
                return new List<DanhSachLichHen>();
            }
        }
        public async Task<List<ThongKeLichHen>> ThongKeLichHen(Guid idChiNhanh, int tenantId)
        {
            using (var command = CreateCommand("prc_dashboard_thongKeLichHen"))
            {
                command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
                command.Parameters.Add(new SqlParameter("@IdChiNhanh", idChiNhanh));
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
        public async Task<List<ThongKeDoanhThu>> ThongKeDoanhThu(Guid idChiNhanh, int tenantId)
        {
            using (var command = CreateCommand("prc_dashboard_thongKeDoanhThu"))
            {
                command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
                command.Parameters.Add(new SqlParameter("@IdChiNhanh", idChiNhanh));
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
        public async Task<List<HotService>> DanhSachDichVuHot(DashboardFilterDto input, int tenantId, long? userId)
        {
            using (var command = CreateCommand("prc_dashboard_hotService"))
            {
                command.Parameters.Add(new SqlParameter("@UserId", userId));
                command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
                command.Parameters.Add(new SqlParameter("@IdChiNhanh", input.IdChiNhanh));
                command.Parameters.Add(new SqlParameter("@ThoiGianTu", input.ThoiGianTu));
                command.Parameters.Add(new SqlParameter("@ThoiGianDen", input.ThoiGianDen));
                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    string[] array = { "Data" };
                    var ds = new DataSet();
                    ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                    var ddd = ds.Tables;

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var data = ObjectHelper.FillCollection<HotService>(ds.Tables[0]);
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                          var tongDoanhThu = ds.Tables[0].Rows[i]["TongDoanhThu"].ToString();
                          data[i].TongDoanhThu = float.Parse(string.IsNullOrEmpty(tongDoanhThu) ? "0": tongDoanhThu);
                          var phanTram = ds.Tables[0].Rows[i]["PhanTram"].ToString();
                          data[i].PhanTram = float.Parse(string.IsNullOrEmpty(phanTram) ? "0": phanTram);
                        }
                        return data;
                    }
                }
                return new List<HotService>();
            }
        }
    }
}
