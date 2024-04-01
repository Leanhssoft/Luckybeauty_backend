using Abp.Application.Services.Dto;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore;
using BanHangBeautify.AppCommon;
using BanHangBeautify.ChietKhau.ChietKhauHoaDon.Dto;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.Entities;
using BanHangBeautify.Entities;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace BanHangBeautify.ChietKhau.ChietKhauHoaDon.Repository
{
    public class ChietKhauHoaDonRepository : SPARepositoryBase<NS_ChietKhauHoaDon, Guid>, IChietKhauHoaDonRepository
    {
        public ChietKhauHoaDonRepository(IDbContextProvider<SPADbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<PagedResultDto<ChietKhauHoaDonItemDto>> GetAll(PagedRequestDto input, int tenantId, Guid? idChinhanh)
        {
            using (var cmd = CreateCommand("prc_chietKhauHoaDon_getAll"))
            {
                cmd.Parameters.Add(new SqlParameter("@TenantId", tenantId));
                cmd.Parameters.Add(new SqlParameter("@IdChiNhanh", idChinhanh));
                cmd.Parameters.Add(new SqlParameter("@Filter", input.Keyword ?? ""));
                cmd.Parameters.Add(new SqlParameter("@SortBy", input.SortBy ?? ""));
                cmd.Parameters.Add(new SqlParameter("@SortType", input.SortType ?? "desc"));
                cmd.Parameters.Add(new SqlParameter("@SkipCount", input.SkipCount));
                cmd.Parameters.Add(new SqlParameter("@MaxResultCount", input.MaxResultCount));
                using (var dataReader = await cmd.ExecuteReaderAsync())
                {
                    string[] array = { "Data", "TotalCount" };
                    var ds = new DataSet();
                    ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {

                        var data = ObjectHelper.FillCollection<ChietKhauHoaDonItemDto>(ds.Tables[0]);

                        return new PagedResultDto<ChietKhauHoaDonItemDto>()
                        {
                            Items = data,
                            TotalCount = int.Parse(ds.Tables[1].Rows[0]["TotalCount"].ToString() ?? "0")
                        };
                    }
                }
            }
            return new PagedResultDto<ChietKhauHoaDonItemDto>()
            {
                Items = null,
                TotalCount = 0
            };
        }

        public async Task<List<ChietKhauHoaDonItemDto>> GetHoaHongNVienSetup_theoLoaiChungTu(Guid idChiNhanh, Guid idNhanVien, string loaiChungTu = "1")
        {
            // todo loaichungtu
            var dbContext = GetDbContext();
            var data = await (from ckhd in dbContext.Set<NS_ChietKhauHoaDon>()
                              join ct in dbContext.Set<NS_ChietKhauHoaDon_ChiTiet>()
                              on ckhd.Id equals ct.IdChietKhauHD
                              where ct.IdNhanVien == idNhanVien
                              && ckhd.IdChiNhanh == idChiNhanh
                              select new ChietKhauHoaDonItemDto
                              {
                                  IdNhanVien = ct.IdNhanVien,
                                  GiaTriChietKhau = ckhd.GiaTriChietKhau,
                                  LoaiChietKhau = ckhd.LoaiChietKhau,
                                  ChungTuApDung = ckhd.ChungTuApDung
                              }).ToListAsync();
            return data;
        }
    }
}
