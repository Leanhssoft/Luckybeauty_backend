using Abp.Application.Services.Dto;
using Abp.EntityFrameworkCore;
using Abp.Runtime.Session;
using BanHangBeautify.Authorization.Users;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using BanHangBeautify.HangHoa.HangHoa.Dto;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BanHangBeautify.Common;
using AutoMapper.Internal.Mappers;

namespace BanHangBeautify.HangHoa.HangHoa.Repository
{
    public class HangHoaRepository : SPARepositoryBase<DM_HangHoa, Guid>, IHangHoaRepository
    {
        public HangHoaRepository(IDbContextProvider<SPADbContext> dbContextProvider)
          : base(dbContextProvider)
        {
        }

        public async Task<PagedResultDto<HangHoaDto>> GetDMHangHoa(HangHoaPagedResultRequestDto input, int? tenantId)
        {
            using (var command = CreateCommand("spGetDMHangHoa"))
            {
                command.Parameters.Add(new SqlParameter("@TenantId", tenantId ?? 1));
                command.Parameters.Add(new SqlParameter("@TextSearch", input.CommonParam.TextSearch ?? (object)DBNull.Value));
                command.Parameters.Add(new SqlParameter("@IdNhomHangHoas", input.IdNhomHangHoas ?? (object)DBNull.Value));
                command.Parameters.Add(new SqlParameter("@CurrentPage", input.CommonParam.CurrentPage));
                command.Parameters.Add(new SqlParameter("@PageSize", input.CommonParam.PageSize));
                command.Parameters.Add(new SqlParameter("@ColumnSort", input.CommonParam.ColumnSort));
                command.Parameters.Add(new SqlParameter("@TypeSort", input.CommonParam.TypeSort));

                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    string[] array = { "Data" };
                    var ds = new DataSet();
                    ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                    var ddd = ds.Tables;

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var data = ObjectHelper.FillCollection<HangHoaDto>(ds.Tables[0]);
                        return new PagedResultDto<HangHoaDto>()
                        {
                            TotalCount = Int32.Parse(ds.Tables[0].Rows[0]["TotalRow"].ToString()),
                            Items = data
                        };
                    }
                }
                return new PagedResultDto<HangHoaDto>();
            }
        }

        public async Task<string> GetProductCode(int loaiHangHoa, int? tenantId)
        {
            using (var command = CreateCommand("select dbo.fnGetProductCode(@TenantId,@LoaiHangHoa) ", System.Data.CommandType.Text))
            {
                command.Parameters.Add(new SqlParameter("@TenantId", tenantId ?? 1));
                command.Parameters.Add(new SqlParameter("@LoaiHangHoa", loaiHangHoa));
                var code = (await command.ExecuteScalarAsync()).ToString();
                return code;
            }
        }
    }
}
