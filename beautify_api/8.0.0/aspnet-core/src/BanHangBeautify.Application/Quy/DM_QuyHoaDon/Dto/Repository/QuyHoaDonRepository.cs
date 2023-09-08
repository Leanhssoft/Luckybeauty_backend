using Abp.Application.Services.Dto;
using Abp.EntityFrameworkCore;
using BanHangBeautify.Configuration.Common;
using BanHangBeautify.Entities;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;


namespace BanHangBeautify.Quy.DM_QuyHoaDon.Dto.Repository
{
    public class QuyHoaDonRepository : SPARepositoryBase<QuyHoaDon, Guid>, IQuyHoaDonRepository
    {
        public QuyHoaDonRepository(IDbContextProvider<SPADbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
        public async Task<string> FnGetMaPhieuThuChi(int tenantId, Guid? idChiNhanh, int idLoaiChungTu, DateTime? ngayLapHoaDon)
        {
            using var command = CreateCommand("select dbo.fnGetMaPhieuThuChi(@TenantId,@IdChiNhanh,@IdLoaiChungTu,@NgayLapHoaDon) ", System.Data.CommandType.Text);
            command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
            command.Parameters.Add(new SqlParameter("@IdChiNhanh", idChiNhanh ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@IdLoaiChungTu", idLoaiChungTu));
            command.Parameters.Add(new SqlParameter("@NgayLapHoaDon", ngayLapHoaDon ?? DateTime.Now));
            var code = (await command.ExecuteScalarAsync()).ToString();
            return code;
        }
        public async Task<PagedResultDto<GetAllQuyHoaDonItemDto>> Search(PagedQuyHoaDonRequestDto input)
        {
            using (var command = CreateCommand("prc_SoQuy_GetAll"))
            {
                string idChiNhanhs = string.Empty;
                if (input.IdChiNhanhs != null && input.IdChiNhanhs.Count > 0)
                {
                    idChiNhanhs = string.Join(",", input.IdChiNhanhs);
                }
                command.Parameters.Add(new SqlParameter("@TenantId", input.TenantId ?? 1));
                command.Parameters.Add(new SqlParameter("@Filter", input.TextSearch ?? ""));
                command.Parameters.Add(new SqlParameter("@IdChiNhanh", idChiNhanhs));
                command.Parameters.Add(new SqlParameter("@FromDate", input.FromDate ?? (object)DBNull.Value));
                command.Parameters.Add(new SqlParameter("@ToDate", input.ToDate ?? (object)DBNull.Value));
                command.Parameters.Add(new SqlParameter("@SortBy", input.ColumnSort ?? "ngayLapHoaDon"));
                command.Parameters.Add(new SqlParameter("@SortType", input.TypeSort ?? "desc"));
                command.Parameters.Add(new SqlParameter("@MaxResultCount", input.PageSize));
                command.Parameters.Add(new SqlParameter("@SkipCount", input.CurrentPage));

                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    string[] array = { "Data" };
                    var ds = new DataSet();
                    ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var data = ObjectHelper.FillCollection<GetAllQuyHoaDonItemDto>(ds.Tables[0]);
                        return new PagedResultDto<GetAllQuyHoaDonItemDto>()
                        {
                            TotalCount = int.Parse(ds.Tables[0].Rows[0]["TotalCount"].ToString()),
                            Items = data
                        };
                    }
                }
                return new PagedResultDto<GetAllQuyHoaDonItemDto>();
            }
        }
        public async Task<List<QuyHoaDonViewItemDto>> GetNhatKyThanhToan_ofHoaDon(Guid idHoaDonLienQuan)
        {
            using (var command = CreateCommand("spGetNhatKyThanhToan_ofHoaDon"))
            {
                command.Parameters.Add(new SqlParameter("@IdHoaDonLienQuan", idHoaDonLienQuan));
                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    string[] array = { "Data" };
                    var ds = new DataSet();
                    ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var data = ObjectHelper.FillCollection<QuyHoaDonViewItemDto>(ds.Tables[0]);
                        return data;
                    }
                }
                return new List<QuyHoaDonViewItemDto>();
            }
        }
    }
}
