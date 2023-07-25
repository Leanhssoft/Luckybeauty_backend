using Abp.Application.Services.Dto;
using Abp.EntityFrameworkCore;
using BanHangBeautify.Common;
using BanHangBeautify.Entities;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using BanHangBeautify.NhanSu.LichLamViec.Dto;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

namespace BanHangBeautify.NhanSu.LichLamViec.Repository
{
    internal class LichLamViecRespository : SPARepositoryBase<NS_LichLamViec, Guid>, ILichLamViecRespository
    {
        public LichLamViecRespository(IDbContextProvider<SPADbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<PagedResultDto<LichLamViecNhanVien>> GetAllLichLamViecWeek(PagedRequestLichLamViecDto input, int tenantId)
        {
            using (var command = CreateCommand("prc_lichLamViec_getAll_Week"))
            {
                command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
                command.Parameters.Add(new SqlParameter("@IdChiNhanh", input.IdChiNhanh));
                command.Parameters.Add(new SqlParameter("@IdNhanVien", input.IdNhanVien));
                command.Parameters.Add(new SqlParameter("@DateFrom", input.DateFrom));
                command.Parameters.Add(new SqlParameter("@DateTo", input.DateTo));
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
                        var data = ObjectHelper.FillCollection<LichLamViecNhanVien>(ds.Tables[0]);
                        return new PagedResultDto<LichLamViecNhanVien>()
                        {
                            TotalCount = int.Parse(ds.Tables[1].Rows[0]["TotalCount"].ToString()),
                            Items = data
                        };
                    }
                }
                return new PagedResultDto<LichLamViecNhanVien>();
            }
        }
    }
}
