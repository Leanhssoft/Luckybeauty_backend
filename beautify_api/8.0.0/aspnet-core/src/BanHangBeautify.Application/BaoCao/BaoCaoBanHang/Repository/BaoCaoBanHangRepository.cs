using Abp.Application.Services.Dto;
using Abp.EntityFrameworkCore;
using BanHangBeautify.BaoCao.BaoCaoBanHang.Dto;
using BanHangBeautify.Configuration.Common;
using BanHangBeautify.Entities;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using BanHangBeautify.NhanSu.NhanVien.Dto;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.BaoCao.BaoCaoBanHang.Repository
{
    public class BaoCaoBanHangRepository : SPARepositoryBase<BH_HoaDon, Guid>, IBaoCaoBanHangRepository
    {
        public BaoCaoBanHangRepository(IDbContextProvider<SPADbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<PagedResultDto<BaoCaoBanHangChiTietDto>> GetBaoCaoBanHangChiTiet(PagedBaoCaoBanHangRequestDto input,int tenantId,DateTime timeFrom,DateTime timeTo)
        {
            using (var command = CreateCommand("prc_baoCao_BanHangChiTiet"))
            {
                command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
                command.Parameters.Add(new SqlParameter("@Filter", input.Filter ?? ""));
                command.Parameters.Add(new SqlParameter("@IdChiNhanh", input.IdChiNhanh));
                command.Parameters.Add(new SqlParameter("@IdDichVu", input.IdDichVu));
                command.Parameters.Add(new SqlParameter("@SortBy", input.SortBy ?? ""));
                command.Parameters.Add(new SqlParameter("@SortType", input.SortType ?? "desc"));
                command.Parameters.Add(new SqlParameter("@MaxResultCount", input.MaxResultCount));
                command.Parameters.Add(new SqlParameter("@SkipCount", input.SkipCount));
                command.Parameters.Add(new SqlParameter("@TimeFrom",timeFrom));
                command.Parameters.Add(new SqlParameter("@TimeTo", timeTo));

                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    string[] array = { "Data", "TotalCount" };
                    var ds = new DataSet();
                    ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                    var ddd = ds.Tables;

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var data = ObjectHelper.FillCollection<BaoCaoBanHangChiTietDto>(ds.Tables[0]);
                        return new PagedResultDto<BaoCaoBanHangChiTietDto>()
                        {
                            TotalCount = int.Parse(ds.Tables[1].Rows[0]["TotalCount"].ToString()),
                            Items = data
                        };
                    }
                }
                return new PagedResultDto<BaoCaoBanHangChiTietDto>();
            }
        }
        public async Task<PagedResultDto<BaoCaoBanHangTongHopDto>> GetBaoCaoBanHangTongHop(PagedBaoCaoBanHangRequestDto input, int tenantId, DateTime timeFrom, DateTime timeTo)
        {
            using (var command = CreateCommand("prc_baoCao_BanHangTongHop"))
            {
                command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
                command.Parameters.Add(new SqlParameter("@Filter", input.Filter ?? ""));
                command.Parameters.Add(new SqlParameter("@IdChiNhanh", input.IdChiNhanh));
                command.Parameters.Add(new SqlParameter("@IdDichVu", input.IdDichVu));
                command.Parameters.Add(new SqlParameter("@SortBy", input.SortBy ?? ""));
                command.Parameters.Add(new SqlParameter("@SortType", input.SortType ?? "desc"));
                command.Parameters.Add(new SqlParameter("@MaxResultCount", input.MaxResultCount));
                command.Parameters.Add(new SqlParameter("@SkipCount", input.SkipCount));
                command.Parameters.Add(new SqlParameter("@TimeFrom", timeFrom));
                command.Parameters.Add(new SqlParameter("@TimeTo", timeTo));

                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    string[] array = { "Data", "TotalCount" };
                    var ds = new DataSet();
                    ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                    var ddd = ds.Tables;

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var data = ObjectHelper.FillCollection<BaoCaoBanHangTongHopDto>(ds.Tables[0]);
                        return new PagedResultDto<BaoCaoBanHangTongHopDto>()
                        {
                            TotalCount = int.Parse(ds.Tables[1].Rows[0]["TotalCount"].ToString()),
                            Items = data
                        };
                    }
                }
                return new PagedResultDto<BaoCaoBanHangTongHopDto>();
            }
        }
    }
}
