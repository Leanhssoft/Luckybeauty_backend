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
using static BanHangBeautify.Common.CommonClass;

namespace BanHangBeautify.HangHoa.HangHoa.Repository
{
    public class HangHoaRepository : SPARepositoryBase<DM_HangHoa, Guid>, IHangHoaRepository
    {
        public HangHoaRepository(IDbContextProvider<SPADbContext> dbContextProvider)
          : base(dbContextProvider)
        {
        }

        public async Task<HangHoaDto> GetDetailProduct(Guid idDonViQuyDoi, int? tenantId)
        {
            using var command = CreateCommand("GetDetailProduct");
            command.Parameters.Add(new SqlParameter("@IdDonViQuyDoi", idDonViQuyDoi));
            using (var dataReader = await command.ExecuteReaderAsync())
            {
                string[] array = { "Data" };
                var ds = new DataSet();
                ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                var ddd = ds.Tables;

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var data = ObjectHelper.FillCollection<HangHoaDto>(ds.Tables[0]).FirstOrDefault();
                    return data;
                }
            }
            return new HangHoaDto();
        }

        public async Task<PagedResultDto<HangHoaDto>> GetDMHangHoa(HangHoaPagedResultRequestDto input, int? tenantId)
        {
            using (var command = CreateCommand("spGetDMHangHoa"))
            {
                command.Parameters.Add(new SqlParameter("@TenantId", tenantId ?? 1));
                command.Parameters.Add(new SqlParameter("@TextSearch", input.ParamSearch.TextSearch ?? (object)DBNull.Value));
                command.Parameters.Add(new SqlParameter("@IdNhomHangHoas", input.IdNhomHangHoas ?? (object)DBNull.Value));
                command.Parameters.Add(new SqlParameter("@Where", DBNull.Value));
                command.Parameters.Add(new SqlParameter("@CurrentPage", input.ParamSearch.CurrentPage));
                command.Parameters.Add(new SqlParameter("@PageSize", input.ParamSearch.PageSize));
                command.Parameters.Add(new SqlParameter("@ColumnSort", input.ParamSearch.ColumnSort));
                command.Parameters.Add(new SqlParameter("@TypeSort", input.ParamSearch.TypeSort));

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

        public async Task<string> GetProductCode(int? loaiHangHoa = 2, int? tenantId = 1)
        {
            using var command = CreateCommand("select dbo.fnGetProductCode(@TenantId,@LoaiHangHoa) ", System.Data.CommandType.Text);
            command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
            command.Parameters.Add(new SqlParameter("@LoaiHangHoa", loaiHangHoa));
            var code = (await command.ExecuteScalarAsync()).ToString();
            return code;
        }

        public async Task<MaxCodeDto> SpGetProductCode(int? tenantId = 1, int? loaiHangHoa = 2)
        {
            using var command = CreateCommand("spGetProductCode");
            command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
            command.Parameters.Add(new SqlParameter("@LoaiHangHoa", loaiHangHoa));

            using (var dataReader = await command.ExecuteReaderAsync())
            {
                string[] array = { "Data" };
                var ds = new DataSet();
                ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                var ddd = ds.Tables;

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var data = ObjectHelper.FillCollection<HangHoaDto>(ds.Tables[0]);
                    return new BanHangBeautify.Common.CommonClass.MaxCodeDto()
                    {
                        FirstStr = ds.Tables[0].Rows[0]["FirstStr"].ToString(),
                        MaxVal = float.Parse(ds.Tables[0].Rows[0]["MaxVal"].ToString()),
                    };
                }
            }
            return new MaxCodeDto();
        }
    }
}
