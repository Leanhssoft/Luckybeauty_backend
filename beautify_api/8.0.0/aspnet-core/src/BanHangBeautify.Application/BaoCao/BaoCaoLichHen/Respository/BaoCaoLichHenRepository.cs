using Abp.Application.Services.Dto;
using Abp.EntityFrameworkCore;
using BanHangBeautify.AppCommon;
using BanHangBeautify.BaoCao.BaoCaoBanHang.Dto;
using BanHangBeautify.BaoCao.BaoCaoLichHen.Dto;
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

namespace BanHangBeautify.BaoCao.BaoCaoLichHen.Respository
{
    public class BaoCaoLichHenRepository : SPARepositoryBase<Booking, Guid>, IBaoCaoLichHenRepository
    {
        public BaoCaoLichHenRepository(IDbContextProvider<SPADbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<PagedResultDto<BaoCaoLichHenDto>> GetBaoCaoLichHen(PagedBaoCaoLichHenRequestDto input, int tenantId)
        {
            using (var command = CreateCommand("prc_baoCao_ThongKeLichHen"))
            {
                command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
                command.Parameters.Add(new SqlParameter("@Filter", input.Filter ?? ""));
                command.Parameters.Add(new SqlParameter("@IdChiNhanh", input.IdChiNhanh));
                command.Parameters.Add(new SqlParameter("@IdDichVu", input.IdDichVu));
                command.Parameters.Add(new SqlParameter("@IdKhachHang", input.IdKhachHang));
                command.Parameters.Add(new SqlParameter("@TrangThai", input.TrangThai));
                command.Parameters.Add(new SqlParameter("@SortBy", input.SortBy ?? ""));
                command.Parameters.Add(new SqlParameter("@SortType", input.SortType ?? "desc"));
                command.Parameters.Add(new SqlParameter("@MaxResultCount", input.MaxResultCount));
                command.Parameters.Add(new SqlParameter("@SkipCount", input.SkipCount));
                command.Parameters.Add(new SqlParameter("@TimeFrom", input.TimeFrom));
                command.Parameters.Add(new SqlParameter("@TimeTo", input.TimeTo));

                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    string[] array = { "Data", "TotalCount" };
                    var ds = new DataSet();
                    ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                    var ddd = ds.Tables;

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var data = ObjectHelper.FillCollection<BaoCaoLichHenDto>(ds.Tables[0]);
                        return new PagedResultDto<BaoCaoLichHenDto>()
                        {
                            TotalCount = int.Parse(ds.Tables[1].Rows[0]["TotalCount"].ToString()),
                            Items = data
                        };
                    }
                }
                return new PagedResultDto<BaoCaoLichHenDto>();
            }
        }
        public async Task<PagedResultDto<BaoCaoKhachHangCheckInDto>> GetBaoCaoKhachHang_CheckIn(ParamSearchBaoCaoCheckin input)
        {
            using var command = CreateCommand("BaoCaoKhachHang_CheckIn");
            string idChiNhanhs = string.Empty, idNhomKhachs = string.Empty;
            if (input.IdChiNhanhs != null && input.IdChiNhanhs.Count > 0)
            {
                idChiNhanhs = string.Join(",", input.IdChiNhanhs);
            }
            if (input.IdNhomKhachs != null && input.IdNhomKhachs.Count > 0)
            {
                idNhomKhachs = string.Join(",", input.IdNhomKhachs);
            }
            command.Parameters.Add(new SqlParameter("@IdChiNhanhs", idChiNhanhs));
            command.Parameters.Add(new SqlParameter("@NgayCheckIn_FromDate", input.FromDate ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@NgayCheckIn_ToDate", input.ToDate ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@IdNhomKhachs", idNhomKhachs));
            command.Parameters.Add(new SqlParameter("@SoNgayChuaCheckIn_From", input.SoNgayChuaCheckIn_From ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@SoNgayChuaCheckIn_To", input.SoNgayChuaCheckIn_To ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@SoLanCheckIn_From", input.SoLanCheckIn_From ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@SoLanCheckIn_To", input.SoLanCheckIn_To ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@SoLanDatHen_From", input.SoLanDatHen_From ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@SoLanDatHen_To", input.SoLanDatHen_To ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@TextSearch", input.TextSearch ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@ColumnSort", input.ColumnSort ?? "TenKhachHang"));
            command.Parameters.Add(new SqlParameter("@TypeSort", input.TypeSort ?? "ASC"));
            command.Parameters.Add(new SqlParameter("@CurrentPage", input.CurrentPage ?? 0));
            command.Parameters.Add(new SqlParameter("@PageSize", input.PageSize ?? 10));

            using (var dataReader = await command.ExecuteReaderAsync())
            {
                string[] array = { "Data" };
                var ds = new DataSet();
                ds.Load(dataReader, LoadOption.OverwriteChanges, array);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var data = ObjectHelper.FillCollection<BaoCaoKhachHangCheckInDto>(ds.Tables[0]);
                    return new PagedResultDto<BaoCaoKhachHangCheckInDto>()
                    {
                        TotalCount = int.Parse(ds.Tables[0].Rows[0]["TotalCount"].ToString()),
                        Items = data
                    };
                }
            }
            return new PagedResultDto<BaoCaoKhachHangCheckInDto>();
        }
    }
}
