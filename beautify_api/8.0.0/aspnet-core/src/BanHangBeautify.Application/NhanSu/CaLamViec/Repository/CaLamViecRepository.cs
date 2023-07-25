using Abp.Application.Services.Dto;
using Abp.EntityFrameworkCore;
using BanHangBeautify.Common;
using BanHangBeautify.Entities;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using BanHangBeautify.NhanSu.CaLamViec.Dto;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

namespace BanHangBeautify.NhanSu.CaLamViec.Repository
{
    public class CaLamViecRepository : SPARepositoryBase<NS_CaLamViec, Guid>, ICaLamViecRepository
    {
        public CaLamViecRepository(IDbContextProvider<SPADbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<PagedResultDto<CaLamViecDto>> GetAll(PagedRequestDto input, int tenantId)
        {
            using (var cmd = CreateCommand("prc_caLamViec_getAll"))
            {
                cmd.Parameters.Add(new SqlParameter("@TenantId", tenantId));
                cmd.Parameters.Add(new SqlParameter("@Filter", input.Keyword));
                cmd.Parameters.Add(new SqlParameter("@SortBy", input.SortBy ?? ""));
                cmd.Parameters.Add(new SqlParameter("@SortType", input.SortType ?? "desc"));
                cmd.Parameters.Add(new SqlParameter("@SkipCount", input.SkipCount));
                cmd.Parameters.Add(new SqlParameter("@MaxResultCount", input.MaxResultCount));
                using (var dataReader = await cmd.ExecuteReaderAsync())
                {
                    string[] arrray = { "Data", "Total" };
                    var ds = new DataSet();
                    ds.Load(dataReader, LoadOption.OverwriteChanges, arrray);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var data = ObjectHelper.FillCollection<CaLamViecDto>(ds.Tables[0]);
                        for (int i = 0; i < data.Count; i++)
                        {
                            data[i].GioVao = ds.Tables[0].Rows[i]["GioVao"].ToString();
                            data[i].GioRa = ds.Tables[0].Rows[i]["GioRa"].ToString();
                        }
                        return new PagedResultDto<CaLamViecDto>()
                        {
                            Items = data,
                            TotalCount = int.Parse(ds.Tables[1].Rows[0]["TotalCount"].ToString())
                        };
                    }
                }
            }
            return new PagedResultDto<CaLamViecDto>()
            {
                Items = null,
                TotalCount = 0
            };
        }
    }
}
