using Abp.EntityFrameworkCore;
using BanHangBeautify.Checkin.Dto;
using BanHangBeautify.Configuration.Common;
using BanHangBeautify.Entities;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using BanHangBeautify.KhachHang.KhachHang.Dto;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BanHangBeautify.Checkin.Repository
{
    public class KHCheckInRespository : SPARepositoryBase<DM_KhachHang, Guid>, IKHCheckInRespository
    {
        private readonly SPADbContext _context;
        public KHCheckInRespository(IDbContextProvider<SPADbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
        public async Task<List<PageKhachHangCheckingDto>> GetListCustomerChecking(PagedKhachHangResultRequestDto input, int? tenantId)
        {
            using (var command = CreateCommand("spGetListCustomerChecking"))
            {
                command.Parameters.Add(new SqlParameter("@TenantId", tenantId ?? 1));
                command.Parameters.Add(new SqlParameter("@IdChiNhanh", input.IdChiNhanh??(object)DBNull.Value));
                command.Parameters.Add(new SqlParameter("@TextSearch", input.keyword ?? ""));
                command.Parameters.Add(new SqlParameter("@CurrentPage", input.SkipCount));
                command.Parameters.Add(new SqlParameter("@PageSize", input.MaxResultCount));

                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    List<PageKhachHangCheckingDto> xx = new();

                    string[] array = { "Data" };
                    var ds = new DataSet();
                    ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                    var ddd = ds.Tables;

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        return ObjectHelper.FillCollection<PageKhachHangCheckingDto>(ds.Tables[0]); ;
                    }
                }
                return new List<PageKhachHangCheckingDto>();
            }
        }
    }
}
