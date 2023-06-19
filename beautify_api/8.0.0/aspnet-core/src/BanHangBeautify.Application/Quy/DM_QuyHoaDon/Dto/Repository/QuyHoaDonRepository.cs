using Abp.Application.Services.Dto;
using Abp.EntityFrameworkCore;
using BanHangBeautify.Common;
using BanHangBeautify.Entities;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using BanHangBeautify.HoaDon.HoaDon.Repository;
using BanHangBeautify.KhachHang.KhachHang.Dto;
using BanHangBeautify.KhachHang.KhachHang.Repository;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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
        public async Task<PagedResultDto<QuyHoaDonViewItemDto>> Search(PagedQuyHoaDonRequestDto input)
        {
            using (var command = CreateCommand("prc_SoQuy_GetAll"))
            {
                command.Parameters.Add(new SqlParameter("@TenantId", input.TenantId??1));
                command.Parameters.Add(new SqlParameter("@Filter", input.Filter ?? ""));
                command.Parameters.Add(new SqlParameter("@IdChiNhanh", input.IdChiNhanh));
                command.Parameters.Add(new SqlParameter("@SortBy", input.SortBy ?? ""));
                command.Parameters.Add(new SqlParameter("@SortType", input.SortType ?? "desc"));
                command.Parameters.Add(new SqlParameter("@MaxResultCount", input.MaxResultCount));
                command.Parameters.Add(new SqlParameter("@SkipCount", input.SkipCount));

                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    string[] array = { "Data", "TotalCount" };
                    var ds = new DataSet();
                    ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var data = ObjectHelper.FillCollection<QuyHoaDonViewItemDto>(ds.Tables[0]);
                        for (int i = 0; i < data.Count; i++)
                        {
                            var tongTienThu = ds.Tables[0].Rows[i]["TongTienThu"].ToString();
                            data[i].TongTienThu = decimal.Parse(string.IsNullOrEmpty(tongTienThu) ? "0" : tongTienThu);
                            data[i].ThoiGianTao = DateTime.Parse(ds.Tables[0].Rows[i]["ThoiGianTao"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        return new PagedResultDto<QuyHoaDonViewItemDto>()
                        {
                            TotalCount = int.Parse(ds.Tables[1].Rows[0]["TotalCount"].ToString()),
                            Items = data
                        };
                    }
                }
                return new PagedResultDto<QuyHoaDonViewItemDto>();
            }
        }
    }
}
