using Abp.Application.Services.Dto;
using Abp.EntityFrameworkCore;
using BanHangBeautify.Common;
using BanHangBeautify.Entities;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using BanHangBeautify.HangHoa.HangHoa.Dto;
using BanHangBeautify.KhachHang.KhachHang.Dto;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.KhachHang.KhachHang.Repository
{
    public class KhachHangRespository : SPARepositoryBase<DM_KhachHang,Guid>, IKhachHangRespository
    {
        public KhachHangRespository(IDbContextProvider<SPADbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<PagedResultDto<KhachHangView>> GetAllKhachHang(PagedKhachHangResultRequestDto input, int? tenantId)
        {
            using (var command = CreateCommand("prc_KhachHang_GetAll"))
            {
                command.Parameters.Add(new SqlParameter("@TenantId", tenantId ?? 1));
                command.Parameters.Add(new SqlParameter("@TextSearch", input.keyword ?? ""));
                command.Parameters.Add(new SqlParameter("@PageSize", input.MaxResultCount));
                command.Parameters.Add(new SqlParameter("@SkipCount", input.SkipCount));

                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    string[] array = { "Data" ,"TotalCount"};
                    var ds = new DataSet();
                    ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                    var ddd = ds.Tables;

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var data = ObjectHelper.FillCollection<KhachHangView>(ds.Tables[0]);
                        return new PagedResultDto<KhachHangView>()
                        {
                            TotalCount = int.Parse(ds.Tables[0].Rows[1]["@totalCount"].ToString()),
                            Items = data
                        };
                    }
                }
                return new PagedResultDto<KhachHangView>();
            }
        }
    }
}
