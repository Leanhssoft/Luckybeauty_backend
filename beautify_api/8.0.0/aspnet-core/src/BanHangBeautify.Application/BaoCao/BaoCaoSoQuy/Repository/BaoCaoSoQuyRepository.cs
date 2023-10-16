using Abp.Application.Services.Dto;
using Abp.EntityFrameworkCore;
using BanHangBeautify.BaoCao.BaoCaoLichHen.Dto;
using BanHangBeautify.BaoCao.BaoCaoSoQuy.Dto;
using BanHangBeautify.Configuration.Common;
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

namespace BanHangBeautify.BaoCao.BaoCaoSoQuy.Repository
{
    public class BaoCaoSoQuyRepository : SPARepositoryBase<QuyHoaDon, Guid>, IBaoCaoSoQuyRepository
    {
        public BaoCaoSoQuyRepository(IDbContextProvider<SPADbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<PagedResultDto<BaoCaoSoQuyDto>> GetBaoCaoSoQuy_NganHang(PagedBaoCaoSoQuyRequestDto input, int tenantId)
        {
            using (var command = CreateCommand("prc_baoCao_BaoCaoSoQuy_NganHang"))
            {
                command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
                command.Parameters.Add(new SqlParameter("@Filter", input.Filter ?? ""));
                command.Parameters.Add(new SqlParameter("@IdChiNhanh", input.IdChiNhanh));
                command.Parameters.Add(new SqlParameter("@SortBy", input.SortBy ?? ""));
                command.Parameters.Add(new SqlParameter("@SortType", input.SortType ?? ""));
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
                        var data = ObjectHelper.FillCollection<BaoCaoSoQuyDto>(ds.Tables[0]);
                        return new PagedResultDto<BaoCaoSoQuyDto>()
                        {
                            TotalCount = int.Parse(ds.Tables[1].Rows[0]["TotalCount"].ToString()),
                            Items = data
                        };
                    }
                }
                return new PagedResultDto<BaoCaoSoQuyDto>();
            }
        }

        public async Task<PagedResultDto<BaoCaoSoQuyDto>> GetBaoCaoSoQuy_TienMat(PagedBaoCaoSoQuyRequestDto input, int tenantId)
        {
            using (var command = CreateCommand("prc_baoCao_BaoCaoSoQuy_TienMat"))
            {
                command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
                command.Parameters.Add(new SqlParameter("@Filter", input.Filter ?? ""));
                command.Parameters.Add(new SqlParameter("@IdChiNhanh", input.IdChiNhanh));
                command.Parameters.Add(new SqlParameter("@SortBy", input.SortBy ?? ""));
                command.Parameters.Add(new SqlParameter("@SortType", input.SortType ?? ""));
                command.Parameters.Add(new SqlParameter("@MaxResultCount", input.MaxResultCount));
                command.Parameters.Add(new SqlParameter("@SkipCount", input.SkipCount));
                command.Parameters.Add(new SqlParameter("@TimeFrom", input.TimeFrom));
                command.Parameters.Add(new SqlParameter("@TimeTo", input.TimeTo));

                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    string[] array = { "Data", "TotalCount" };
                    var ds = new DataSet();
                    ds.Load(dataReader, LoadOption.OverwriteChanges, array);

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var data = ObjectHelper.FillCollection<BaoCaoSoQuyDto>(ds.Tables[0]);
                        return new PagedResultDto<BaoCaoSoQuyDto>()
                        {
                            TotalCount = int.Parse(ds.Tables[1].Rows[0]["TotalCount"].ToString()),
                            Items = data
                        };
                    }
                }
            }

            return new PagedResultDto<BaoCaoSoQuyDto>();
        }
    }
}
