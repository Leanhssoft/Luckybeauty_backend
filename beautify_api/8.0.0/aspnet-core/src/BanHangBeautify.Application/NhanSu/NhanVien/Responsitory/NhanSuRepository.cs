using Abp.Application.Services.Dto;
using Abp.EntityFrameworkCore;
using BanHangBeautify.Common;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using BanHangBeautify.KhachHang.KhachHang.Dto;
using BanHangBeautify.NhanSu.NhanVien.Dto;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.NhanSu.NhanVien.Responsitory
{
    public class NhanSuRepository : SPARepositoryBase<NS_NhanVien, Guid>, INhanSuRepository
    {
        public NhanSuRepository(IDbContextProvider<SPADbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<PagedResultDto<NhanSuItemDto>> GetAllNhanSu(PagedNhanSuRequestDto input)
        {
            using (var command = CreateCommand("prc_nhanVien_getAll"))
            {
                command.Parameters.Add(new SqlParameter("@TenantId", input.TenantId));
                command.Parameters.Add(new SqlParameter("@Filter", input.Filter ?? ""));
                command.Parameters.Add(new SqlParameter("@IdChiNhanh", input.IdChiNhanh));
                command.Parameters.Add(new SqlParameter("@SortBy", input.SortBy ?? ""));
                command.Parameters.Add(new SqlParameter("@SortType", input.SortBy ?? "desc"));
                command.Parameters.Add(new SqlParameter("@MaxResultCount", input.MaxResultCount));
                command.Parameters.Add(new SqlParameter("@SkipCount", input.SkipCount));

                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    string[] array = { "Data", "TotalCount" };
                    var ds = new DataSet();
                    ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                    var ddd = ds.Tables;

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var data = ObjectHelper.FillCollection<NhanSuItemDto>(ds.Tables[0]);
                        return new PagedResultDto<NhanSuItemDto>()
                        {
                            TotalCount = int.Parse(ds.Tables[1].Rows[0]["TotalCount"].ToString()),
                            Items = data
                        };
                    }
                }
                return new PagedResultDto<NhanSuItemDto>();
            }
        }
    }
}
