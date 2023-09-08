using Abp.Application.Services.Dto;
using Abp.EntityFrameworkCore;
using BanHangBeautify.ChietKhau.ChietKhauHoaDon.Dto;
using BanHangBeautify.Configuration.Common;
using BanHangBeautify.Entities;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

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
    }
}
