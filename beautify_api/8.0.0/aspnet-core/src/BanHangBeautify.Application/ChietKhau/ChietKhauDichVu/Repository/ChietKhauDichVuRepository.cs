using Abp.Application.Services.Dto;
using Abp.EntityFrameworkCore;
using BanHangBeautify.ChietKhau.ChietKhauDichVu.Dto;
using BanHangBeautify.Configuration.Common;
using BanHangBeautify.Entities;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.ChietKhau.ChietKhauDichVu.Repository
{
    internal class ChietKhauDichVuRepository : SPARepositoryBase<NS_ChietKhauDichVu, Guid>, IChietKhauDichVuRepository
    {
        public ChietKhauDichVuRepository(IDbContextProvider<SPADbContext> dbContextProvider)
        : base(dbContextProvider)
        {
        }

        public async Task<PagedResultDto<ChietKhauDichVuItemDto>> GetAll(PagedRequestDto input, int tenantId, Guid idNhanVien, Guid idChiNhanh)
        {
            using (var cmd = CreateCommand("prc_chietKhauDichVu_getAll"))
            {
                cmd.Parameters.Add(new SqlParameter("@TenantId", tenantId));
                cmd.Parameters.Add(new SqlParameter("@IdChiNhanh", idChiNhanh));
                cmd.Parameters.Add(new SqlParameter("@IdNhanVien", idNhanVien));
                cmd.Parameters.Add(new SqlParameter("@Filter", input.Keyword ?? ""));
                cmd.Parameters.Add(new SqlParameter("@SortBy", input.SortBy ?? ""));
                cmd.Parameters.Add(new SqlParameter("@SortType", input.SortType ?? "desc"));
                cmd.Parameters.Add(new SqlParameter("@SkipCount", input.SkipCount));
                cmd.Parameters.Add(new SqlParameter("@MaxResultCount", input.MaxResultCount));
                using (var dataReadder = await cmd.ExecuteReaderAsync())
                {
                    string[] array = { "Data", "TotalCount" };
                    var ds = new DataSet();
                    ds.Load(dataReadder, LoadOption.OverwriteChanges, array);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var data = ObjectHelper.FillCollection<ChietKhauDichVuItemDto>(ds.Tables[0]);
                        for (int i = 0; i < data.Count; i++)
                        {
                            var hoaHongTuVan = ds.Tables[0].Rows[i]["HoaHongTuVan"].ToString();
                            var hoaHongThucHien = ds.Tables[0].Rows[i]["HoaHongThucHien"].ToString();
                            var hoaHongYCThucHien = ds.Tables[0].Rows[i]["HoaHongYeuCauThucHien"].ToString();
                            data[i].HoaHongTuVan = float.Parse(string.IsNullOrEmpty(hoaHongTuVan) ? "0" : hoaHongTuVan);
                            data[i].HoaHongYeuCauThucHien = float.Parse(string.IsNullOrEmpty(hoaHongYCThucHien) ? "0" : hoaHongYCThucHien);
                            data[i].HoaHongThucHien = float.Parse(string.IsNullOrEmpty(hoaHongThucHien) ? "0" : hoaHongThucHien);
                        }
                        return new PagedResultDto<ChietKhauDichVuItemDto>()
                        {
                            Items = data,
                            TotalCount = int.Parse(ds.Tables[1].Rows[0]["TotalCount"].ToString() ?? "0")
                        };
                    }
                }
            }
            return new PagedResultDto<ChietKhauDichVuItemDto>();
        }

        public async Task<ChietKhauDichVuDto> GetHoaHongNV_theoDichVu(int tenantId, Guid idNhanVien, Guid idDonViQuyDoi)
        {
            using var command = CreateCommand("GetHoaHongNV_theoDichVu");
            command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
            command.Parameters.Add(new SqlParameter("@IdNhanVien", idNhanVien));
            command.Parameters.Add(new SqlParameter("@IdDonViQuyDoi", idDonViQuyDoi));
            using (var dataReader = await command.ExecuteReaderAsync())
            {
                string[] array = { "Data" };
                var ds = new DataSet();
                ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                var ddd = ds.Tables;

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var data = ObjectHelper.FillCollection<ChietKhauDichVuDto>(ds.Tables[0]).FirstOrDefault();
                    return data;
                }
            }
            return new ChietKhauDichVuDto();
        }

    }
}
