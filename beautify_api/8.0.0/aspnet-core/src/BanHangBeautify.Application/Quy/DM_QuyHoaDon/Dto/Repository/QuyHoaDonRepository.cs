using Abp.Application.Services.Dto;
using Abp.EntityFrameworkCore;
using BanHangBeautify.AppCommon;
using BanHangBeautify.Entities;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using BanHangBeautify.Quy.QuyHoaDonChiTiet.Dto;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
            using var command = CreateCommand("spGetAllSoQuy");
            string idChiNhanhs = string.Empty, hinhThucThanhToans = string.Empty;
            if (input.IdChiNhanhs != null && input.IdChiNhanhs.Count > 0)
            {
                idChiNhanhs = string.Join(",", input.IdChiNhanhs);
            }
            if (input.HinhThucThanhToans != null && input.HinhThucThanhToans.Count > 0)
            {
                hinhThucThanhToans = string.Join(",", input.HinhThucThanhToans);
            }
            command.Parameters.Add(new SqlParameter("@TenantId", input.TenantId ?? 1));
            command.Parameters.Add(new SqlParameter("@IdChiNhanhs", idChiNhanhs));
            command.Parameters.Add(new SqlParameter("@HinhThucThanhToans", hinhThucThanhToans));
            command.Parameters.Add(new SqlParameter("@IdKhoanThuChi", input?.IdKhoanThuChi ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@IdTaiKhoanNganHang", input?.IdTaiKhoanNganHang ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@FromDate", input.FromDate ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@ToDate", input.ToDate ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@Filter", input.TextSearch ?? ""));
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
                        TotalCount = int.Parse(ds.Tables[0].Rows[0]["TotalRow"].ToString()),
                        Items = data
                    };
                }
            }
            return new PagedResultDto<GetAllQuyHoaDonItemDto>();
        }
        public async Task<ThuChi_DauKyCuoiKyDto> GetThuChi_DauKyCuoiKy(PagedQuyHoaDonRequestDto input)
        {
            using var command = CreateCommand("GetThuChi_DauKyCuoiKy");
            string idChiNhanhs = string.Empty, hinhThucThanhToans = string.Empty;
            if (input.IdChiNhanhs != null && input.IdChiNhanhs.Count > 0)
            {
                idChiNhanhs = string.Join(",", input.IdChiNhanhs);
            }
            if (input.HinhThucThanhToans != null && input.HinhThucThanhToans.Count > 0)
            {
                hinhThucThanhToans = string.Join(",", input.HinhThucThanhToans);
            }
            command.Parameters.Add(new SqlParameter("@IdChiNhanhs", idChiNhanhs));
            command.Parameters.Add(new SqlParameter("@HinhThucThanhToans", hinhThucThanhToans));
            command.Parameters.Add(new SqlParameter("@IdKhoanThuChi", input?.IdKhoanThuChi ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@IdTaiKhoanNganHang", input?.IdTaiKhoanNganHang ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@FromDate", input.FromDate ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@ToDate", input.ToDate ?? (object)DBNull.Value));

            using (var dataReader = await command.ExecuteReaderAsync())
            {
                string[] array = { "Data" };
                var ds = new DataSet();
                ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var data = ObjectHelper.FillCollection<ThuChi_DauKyCuoiKyDto>(ds.Tables[0]);
                    return data.FirstOrDefault();
                }
            }
            return new ThuChi_DauKyCuoiKyDto();
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
        public async Task<List<QuyHoaDonChiTietDto>> GetQuyChiTiet_byIQuyHoaDon(Guid idQuyHoaDon)
        {
            using (var command = CreateCommand("spGetQuyChiTiet_byIQuyHoaDon"))
            {
                command.Parameters.Add(new SqlParameter("@IdQuyHoaDon", idQuyHoaDon));
                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    string[] array = { "Data" };
                    var ds = new DataSet();
                    ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var data = ObjectHelper.FillCollection<QuyHoaDonChiTietDto>(ds.Tables[0]);
                        return data;
                    }
                }
                return new List<QuyHoaDonChiTietDto>();
            }
        }
    }
}
