using Abp.Application.Services.Dto;
using Abp.EntityFrameworkCore;
using BanHangBeautify.AppCommon;
using BanHangBeautify.ChietKhau.ChietKhauDichVu.Dto;
using BanHangBeautify.ChietKhau.ChietKhauHoaDon.Dto;
using BanHangBeautify.Entities;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
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

        public async Task<PagedResultDto<ChietKhauDichVuItemDto>> GetAll(PagedRequestDto input, int tenantId,
            Guid? idNhanVien = null, Guid? idChiNhanh = null)
        {
            using (var cmd = CreateCommand("prc_chietKhauDichVu_getAll"))
            {
                cmd.Parameters.Add(new SqlParameter("@TenantId", tenantId));
                cmd.Parameters.Add(new SqlParameter("@IdChiNhanh", idChiNhanh));
                cmd.Parameters.Add(new SqlParameter("@IdNhanVien", idNhanVien ?? (object)DBNull.Value));
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

        public async Task<PagedResultDto<ChietKhauDichVuItemDto_TachRiengCot>> GetAllSetup_HoaHongDichVu(PagedRequestDto input, int tenantId,
            Guid? idNhanVien = null, Guid? idChiNhanh = null)
        {
            using (var cmd = CreateCommand("GetAllSetup_HoaHongDichVu"))
            {
                cmd.Parameters.Add(new SqlParameter("@TenantId", tenantId));
                cmd.Parameters.Add(new SqlParameter("@IdChiNhanh", idChiNhanh));
                cmd.Parameters.Add(new SqlParameter("@IdNhanVien", idNhanVien ?? (object)DBNull.Value));
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
                        var data = ObjectHelper.FillCollection<ChietKhauDichVuItemDto_TachRiengCot>(ds.Tables[0]);
                        return new PagedResultDto<ChietKhauDichVuItemDto_TachRiengCot>()
                        {
                            Items = data,
                            TotalCount = int.Parse(ds.Tables[1].Rows[0]["TotalCount"].ToString() ?? "0")
                        };
                    }
                }
            }
            return new PagedResultDto<ChietKhauDichVuItemDto_TachRiengCot>();
        }
        public async Task<int> AddMultiple_ChietKhauDichVu_toMultipleNhanVien(ChietKhauDichVuDto_AddMultiple param, int tenantId)
        {
            string idNhanViens = string.Empty, idQuyDois = string.Empty;
            if (param.IdNhanViens != null && param.IdNhanViens.Count > 0)
            {
                idNhanViens = string.Join(",", param.IdNhanViens);
            }
            if (param.IdDonViQuyDois != null && param.IdDonViQuyDois.Count > 0)
            {
                idQuyDois = string.Join(",", param.IdDonViQuyDois);
            }

            if (string.IsNullOrEmpty(idNhanViens) && string.IsNullOrEmpty(idQuyDois)) return 0;
            if (param.IdChiNhanh == null || param.IdChiNhanh == Guid.Empty) return 0;

            using var cmd = CreateCommand("AddMultiple_ChietKhauDichVu_toMultipleNhanVien");
            cmd.Parameters.Add(new SqlParameter("@TenantId", tenantId));
            cmd.Parameters.Add(new SqlParameter("@IdChiNhanh", param.IdChiNhanh));
            cmd.Parameters.Add(new SqlParameter("@IdNhanViens", idNhanViens));
            cmd.Parameters.Add(new SqlParameter("@IdDonViQuyDois", idQuyDois));
            cmd.Parameters.Add(new SqlParameter("@LoaiChietKhau", param.LoaiChietKhau ?? 1));
            cmd.Parameters.Add(new SqlParameter("@GiaTriChietKhau", param.GiaTri ?? 0));
            cmd.Parameters.Add(new SqlParameter("@LaPhanTram", param.LaPhanTram ?? true));

            var countOK = await cmd.ExecuteNonQueryAsync();
            return countOK;
        }
    }
}
