using Abp.Application.Services.Dto;
using Abp.EntityFrameworkCore;
using BanHangBeautify.Common;
using BanHangBeautify.Entities;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using BanHangBeautify.NhanSu.NhanVien.Dto;
using BanHangBeautify.Suggests.Dto;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BanHangBeautify.Suggests.Repository
{
    public class SuggestRepository : SPARepositoryBase<DM_ChiNhanh, Guid>, ISuggestRepository
    {
        public SuggestRepository(IDbContextProvider<SPADbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<List<SuggestEmpolyeeExecuteServiceDto>> SuggestNhanVienThucHienDichVu(int tenantId, Guid idChiNhanh)
        {
            using(var cmd = CreateCommand("prc_SuggestNhanVienDichVu"))
            {
                cmd.Parameters.Add(new SqlParameter("@TenantId", tenantId));
                cmd.Parameters.Add(new SqlParameter("@IdChiNhanh", idChiNhanh));
                using (var dataReader = await cmd.ExecuteReaderAsync())
                {
                    string[] array = { "Data", "TotalCount" };
                    var ds = new DataSet();
                    ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                    var ddd = ds.Tables;

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var data = ObjectHelper.FillCollection<SuggestEmpolyeeExecuteServiceDto>(ds.Tables[0]);
                        return data;
                    }
                }
                return new List<SuggestEmpolyeeExecuteServiceDto>();
            }
        }
    }
}
