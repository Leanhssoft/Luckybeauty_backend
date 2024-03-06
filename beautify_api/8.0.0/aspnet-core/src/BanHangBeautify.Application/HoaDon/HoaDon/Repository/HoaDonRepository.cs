using Abp.Application.Services.Dto;
using Abp.EntityFrameworkCore;
using BanHangBeautify.AppCommon;
using BanHangBeautify.Entities;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using BanHangBeautify.HoaDon.HoaDon.Dto;
using BanHangBeautify.HoaDon.HoaDonChiTiet.Dto;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BanHangBeautify.HoaDon.HoaDon.Repository
{
    public class HoaDonRepository : SPARepositoryBase<BH_HoaDon, Guid>, IHoaDonRepository
    {
        public HoaDonRepository(IDbContextProvider<SPADbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
        public async Task<string> GetMaHoaDon(int tenantId, Guid? idChiNhanh, int idLoaiChungTu, DateTime ngayLapHoaDon)
        {
            using (var command = CreateCommand("spGetMaHoaDon"))
            {
                command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
                command.Parameters.Add(new SqlParameter("@IdChiNhanh", idChiNhanh));
                command.Parameters.Add(new SqlParameter("@IdLoaiChungTu", idLoaiChungTu));
                command.Parameters.Add(new SqlParameter("@NgayLapHoaDon", ngayLapHoaDon));

                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    string[] array = { "Data" };
                    var ds = new DataSet();
                    ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                    var ddd = ds.Tables;

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var cxx = ObjectHelper.FillCollection<string>(ds.Tables[0]).ToString();
                        return ObjectHelper.FillCollection<string>(ds.Tables[0]).ToString();
                    }
                }
                return string.Empty;
            }
        }
        public async Task<string> FnGetMaHoaDon(int tenantId, Guid? idChiNhanh, int idLoaiChungTu, DateTime? ngayLapHoaDon)
        {
            using var command = CreateCommand("select dbo.fnGetMaHoaDon(@TenantId,@IdChiNhanh,@IdLoaiChungTu,@NgayLapHoaDon) ", System.Data.CommandType.Text);
            command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
            command.Parameters.Add(new SqlParameter("@IdChiNhanh", idChiNhanh ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@IdLoaiChungTu", idLoaiChungTu));
            command.Parameters.Add(new SqlParameter("@NgayLapHoaDon", ngayLapHoaDon ?? DateTime.Now));
            var code = (await command.ExecuteScalarAsync()).ToString();
            return code;
        }
        public async Task<PagedResultDto<PageHoaDonDto>> GetListHoaDon(HoaDonRequestDto param, int? tenantId = 1)
        {
            string idChiNhanhs = string.Empty, idLoaiChungTus = string.Empty,
                trangThaiHDs = string.Empty, trangThaiNos = string.Empty;
            if (param.IdChiNhanhs != null && param.IdChiNhanhs.Count > 0)
            {
                idChiNhanhs = string.Join(",", param.IdChiNhanhs);
            }
            if (param.IdLoaiChungTus != null && param.IdLoaiChungTus.Count > 0)
            {
                idLoaiChungTus = string.Join(",", param.IdLoaiChungTus);
            }
            if (param.TrangThais != null && param.TrangThais.Count > 0)
            {
                trangThaiHDs = string.Join(",", param.TrangThais);
            }
            if (param.TrangThaiNos != null && param.TrangThaiNos.Count > 0)
            {
                trangThaiNos = string.Join(",", param.TrangThaiNos);
            }
            using var command = CreateCommand("spGetListHoaDon");
            command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
            command.Parameters.Add(new SqlParameter("@IdChiNhanhs", idChiNhanhs ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@IdUserLogin", param.IdUserLogin));
            command.Parameters.Add(new SqlParameter("@IdLoaiChungTus", idLoaiChungTus));
            command.Parameters.Add(new SqlParameter("@TrangThaiHDs", trangThaiHDs));
            command.Parameters.Add(new SqlParameter("@TrangThaiNos", trangThaiNos));
            command.Parameters.Add(new SqlParameter("@DateFrom", param.FromDate ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@DateTo", param.ToDate ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@TextSearch", param.TextSearch ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@CurrentPage", param.CurrentPage));
            command.Parameters.Add(new SqlParameter("@PageSize", param.PageSize));
            command.Parameters.Add(new SqlParameter("@ColumnSort", param.ColumnSort));
            command.Parameters.Add(new SqlParameter("@TypeSort", param.TypeSort));

            using (var dataReader = await command.ExecuteReaderAsync())
            {
                string[] array = { "Data" };
                var ds = new DataSet();
                ds.Load(dataReader, LoadOption.OverwriteChanges, array);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var data = ObjectHelper.FillCollection<PageHoaDonDto>(ds.Tables[0]);
                    return new PagedResultDto<PageHoaDonDto>()
                    {
                        TotalCount = Int32.Parse(ds.Tables[0].Rows[0]["TotalRow"].ToString()),
                        Items = data
                    };
                }
            }
            return new PagedResultDto<PageHoaDonDto>();
        }

        public async Task<List<PageHoaDonDto>> GetInforHoaDon_byId(Guid id)
        {
            using var command = CreateCommand("spGetInforHoaDon_byId");
            command.Parameters.Add(new SqlParameter("@Id", id));

            using (var dataReader = await command.ExecuteReaderAsync())
            {
                string[] array = { "Data" };
                var ds = new DataSet();
                ds.Load(dataReader, LoadOption.OverwriteChanges, array);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var data = ObjectHelper.FillCollection<PageHoaDonDto>(ds.Tables[0]);
                    return data;
                }
            }
            return new List<PageHoaDonDto>();
        }
        public async Task<List<PageHoaDonChiTietDto>> GetChiTietHoaDon_byIdHoaDon(Guid idHoaDon)
        {
            using var command = CreateCommand("spGetChiTietHoaDon_byIdHoaDon");
            command.Parameters.Add(new SqlParameter("@IdHoaDon", idHoaDon));

            using (var dataReader = await command.ExecuteReaderAsync())
            {
                string[] array = { "Data" };
                var ds = new DataSet();
                ds.Load(dataReader, LoadOption.OverwriteChanges, array);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var data = ObjectHelper.FillCollection<PageHoaDonChiTietDto>(ds.Tables[0]);
                    return data;
                }
            }
            return new List<PageHoaDonChiTietDto>();
        }
    }
}
