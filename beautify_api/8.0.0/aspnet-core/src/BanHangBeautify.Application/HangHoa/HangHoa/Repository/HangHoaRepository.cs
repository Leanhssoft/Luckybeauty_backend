﻿using Abp.Application.Services.Dto;
using Abp.EntityFrameworkCore;
using BanHangBeautify.AppCommon;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using BanHangBeautify.HangHoa.HangHoa.Dto;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static BanHangBeautify.AppCommon.CommonClass;

namespace BanHangBeautify.HangHoa.HangHoa.Repository
{
    public class HangHoaRepository : SPARepositoryBase<DM_HangHoa, Guid>, IHangHoaRepository
    {
        public HangHoaRepository(IDbContextProvider<SPADbContext> dbContextProvider)
          : base(dbContextProvider)
        {
        }

        public async Task<HangHoaDto> GetDetailProduct(Guid idDonViQuyDoi, int? tenantId)
        {
            using var command = CreateCommand("GetDetailProduct");
            command.Parameters.Add(new SqlParameter("@IdDonViQuyDoi", idDonViQuyDoi));
            using (var dataReader = await command.ExecuteReaderAsync())
            {
                string[] array = { "Data" };
                var ds = new DataSet();
                ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                var ddd = ds.Tables;

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var data = ObjectHelper.FillCollection<HangHoaDto>(ds.Tables[0]).FirstOrDefault();
                    return data;
                }
            }
            return new HangHoaDto();
        }

        public async Task<PagedResultDto<HangHoaDto>> GetDMHangHoa(HangHoaRequestDto input, int? tenantId)
        {
            using (var command = CreateCommand("spGetDMHangHoa"))
            {
                string idNhomHangs = string.Empty;
                if (input?.IdNhomHangHoas != null && input?.IdNhomHangHoas?.Count > 0)
                {
                    idNhomHangs = string.Join(",", input.IdNhomHangHoas);
                }
                command.Parameters.Add(new SqlParameter("@TenantId", tenantId ?? 1));
                command.Parameters.Add(new SqlParameter("@TextSearch", input.TextSearch ?? (object)DBNull.Value));
                command.Parameters.Add(new SqlParameter("@IdNhomHangHoas", idNhomHangs ?? (object)DBNull.Value));
                command.Parameters.Add(new SqlParameter("@IdLoaiHangHoa", input.IdLoaiHangHoa ?? 0));
                command.Parameters.Add(new SqlParameter("@Where", DBNull.Value));
                command.Parameters.Add(new SqlParameter("@CurrentPage", input.CurrentPage));
                command.Parameters.Add(new SqlParameter("@PageSize", input.PageSize));
                command.Parameters.Add(new SqlParameter("@ColumnSort", input.ColumnSort));
                command.Parameters.Add(new SqlParameter("@TypeSort", input.TypeSort));

                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    string[] array = { "Data" };
                    var ds = new DataSet();
                    ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                    var ddd = ds.Tables;

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var data = ObjectHelper.FillCollection<HangHoaDto>(ds.Tables[0]);
                        return new PagedResultDto<HangHoaDto>()
                        {
                            TotalCount = Int32.Parse(ds.Tables[0].Rows[0]["TotalRow"].ToString()),
                            Items = data
                        };
                    }
                }
                return new PagedResultDto<HangHoaDto>();
            }
        }

        public async Task<string> GetProductCode(int? loaiHangHoa = 2, int? tenantId = 1)
        {
            using var command = CreateCommand("select dbo.fnGetProductCode(@TenantId,@LoaiHangHoa) ", System.Data.CommandType.Text);
            command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
            command.Parameters.Add(new SqlParameter("@LoaiHangHoa", loaiHangHoa));
            var code = (await command.ExecuteScalarAsync()).ToString();
            return code;
        }

        public async Task<MaxCodeDto> SpGetProductCode(int? loaiHangHoa = 2, int? tenantId = 1)
        {
            using var command = CreateCommand("spGetProductCode");
            command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
            command.Parameters.Add(new SqlParameter("@LoaiHangHoa", loaiHangHoa));

            using (var dataReader = await command.ExecuteReaderAsync())
            {
                string[] array = { "Data" };
                var ds = new DataSet();
                ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                var ddd = ds.Tables;

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var data = ObjectHelper.FillCollection<HangHoaDto>(ds.Tables[0]);
                    return new CommonClass.MaxCodeDto()
                    {
                        FirstStr = ds.Tables[0].Rows[0]["FirstStr"].ToString(),
                        MaxVal = float.Parse(ds.Tables[0].Rows[0]["MaxVal"].ToString()),
                    };
                }
            }
            return new MaxCodeDto();
        }

        public async Task ImportDanhMucHangHoa(int? tenantId, long? userId, ImportExcelHangHoaDto dataHangHoa)
        {
            using var command = CreateCommand("spImportDanhMucHangHoa");
            command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
            command.Parameters.Add(new SqlParameter("@CreatorUserId", userId));
            command.Parameters.Add(new SqlParameter("@TenNhomHangHoa", dataHangHoa.TenNhomHangHoa ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@MaHangHoa", dataHangHoa.MaHangHoa ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@TenHangHoa", dataHangHoa.TenHangHoa ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@IdLoaiHangHoa", dataHangHoa.IdLoaiHangHoa));
            command.Parameters.Add(new SqlParameter("@GiaBan", dataHangHoa.GiaBan ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@SoPhutThucHien", dataHangHoa.SoPhutThucHien ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@GhiChu", dataHangHoa.GhiChu ?? (object)DBNull.Value));
            await command.ExecuteNonQueryAsync();
        }
        /// <summary>
        /// lấy thông tin đường dẫn ảnh bất kỳ của hàng hóa (sử dụng cho Imgur)
        /// data Image được lưu dưới dạng: imageId-deleteHashImage/albumId-deleteHashAlbum
        /// Ảnh trên Imgur được lưu theo cấu trúc: tennantName_HangHoa/image1,image2,...; tennantName_KhachHang/... (vì imgur không có API tạo subFoder)
        /// </summary>
        /// <returns></returns>
        public string GetInforImage_OfAnyHangHoa()
        {
            var dbContext = GetDbContext();
            var data = dbContext.Set<DM_HangHoa>().Where(x => !string.IsNullOrEmpty(x.Image) && x.TrangThai == 1).FirstOrDefault();
            if (data != null)
            {
                return data.Image;
            }
            return string.Empty;
        }
    }
}
