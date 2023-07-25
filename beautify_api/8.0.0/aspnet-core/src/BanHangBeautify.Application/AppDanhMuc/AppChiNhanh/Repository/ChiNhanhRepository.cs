using Abp.Application.Services.Dto;
using Abp.EntityFrameworkCore;
using BanHangBeautify.AppDanhMuc.AppChiNhanh.Dto;
using BanHangBeautify.Common;
using BanHangBeautify.Entities;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDanhMuc.AppChiNhanh.Repository
{
    public class ChiNhanhRepository : SPARepositoryBase<DM_ChiNhanh, Guid>, IChiNhanhRepository
    {
        public ChiNhanhRepository(IDbContextProvider<SPADbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<PagedResultDto<ChiNhanhDto>> GetAll(PagedRequestDto input, int tenantId)
        {
            using (var cmd = CreateCommand("prc_chiNhanh_getAll"))
            {
                cmd.Parameters.Add(new SqlParameter("TenantId", tenantId));
                cmd.Parameters.Add(new SqlParameter("Filter", input.Keyword ?? ""));
                cmd.Parameters.Add(new SqlParameter("SortBy", input.SortBy ?? ""));
                cmd.Parameters.Add(new SqlParameter("SortType", input.SortType ?? "desc"));
                cmd.Parameters.Add(new SqlParameter("SkipCount", input.SkipCount));
                cmd.Parameters.Add(new SqlParameter("MaxResultCount", input.MaxResultCount));
                using (var dataReader = await cmd.ExecuteReaderAsync())
                {
                    string[] array = { "Data", "TotalCount" };
                    var ds = new DataSet();
                    ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var data = ObjectHelper.FillCollection<ChiNhanhDto>(ds.Tables[0]);

                        return new PagedResultDto<ChiNhanhDto>()
                        {
                            TotalCount = int.Parse(ds.Tables[1].Rows[0]["TotalCount"].ToString()),
                            Items = data
                        };
                    }
                    return new PagedResultDto<ChiNhanhDto>() { Items = null, TotalCount = 0 };

                }

            }
        }
    }
}
