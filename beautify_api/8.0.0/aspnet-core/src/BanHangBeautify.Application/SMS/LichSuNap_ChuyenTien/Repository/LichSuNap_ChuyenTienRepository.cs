using Abp.Application.Services.Dto;
using Abp.EntityFrameworkCore;
using BanHangBeautify.AppCommon;
using BanHangBeautify.ChietKhau.ChietKhauHoaDon.Dto;
using BanHangBeautify.ChietKhau.ChietKhauHoaDon.Repository;
using BanHangBeautify.Entities;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using BanHangBeautify.SMS.Dto;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BanHangBeautify.AppCommon.CommonClass;

namespace BanHangBeautify.SMS.LichSuNap_ChuyenTien.Repository
{
    public class LichSuNap_ChuyenTienRepository : SPARepositoryBase<SMS_LichSuNap_ChuyenTien, Guid>, ILichSuNap_ChuyenTienRepository
    {
        public LichSuNap_ChuyenTienRepository(IDbContextProvider<SPADbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<PagedResultDto<PageNhatKyChuyenTienDto>> GetAllNhatKyChuyenTien(ParamSearch param)
        {
            using (var cmd = CreateCommand("GetAllNhatKyChuyenTien"))
            {
                cmd.Parameters.Add(new SqlParameter("@TextSearch", param.TextSearch ?? (object)DBNull.Value)); ;
                cmd.Parameters.Add(new SqlParameter("@CurrentPage", param.CurrentPage ?? 0));
                cmd.Parameters.Add(new SqlParameter("@PageSize", param.PageSize ?? 10));
                cmd.Parameters.Add(new SqlParameter("@ColumnSort", param.ColumnSort ?? "createTime"));
                cmd.Parameters.Add(new SqlParameter("@TypeSort", param.TypeSort ?? "desc"));
                using var dataReadder = await cmd.ExecuteReaderAsync();
                string[] array = { "Data", "TotalCount" };
                var ds = new DataSet();
                ds.Load(dataReadder, LoadOption.OverwriteChanges, array);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var data = ObjectHelper.FillCollection<PageNhatKyChuyenTienDto>(ds.Tables[0]);
                    return new PagedResultDto<PageNhatKyChuyenTienDto>()
                    {
                        Items = data,
                        TotalCount = int.Parse(ds.Tables[1].Rows[0]["TotalCount"].ToString() ?? "0")
                    };
                }
            }
            return new PagedResultDto<PageNhatKyChuyenTienDto>();
        }
    }
}
