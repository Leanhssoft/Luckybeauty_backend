using Abp.Application.Services.Dto;
using Abp.EntityFrameworkCore;
using BanHangBeautify.AppCommon;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.Entities;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using BanHangBeautify.HangHoa.HangHoa.Repository;
using BanHangBeautify.SMS.Dto;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.SMS.Brandname.Repository
{
    public class BrandnameRepository : SPARepositoryBase<HT_SMSBrandname, Guid>, IBrandnameRepository
    {
        public BrandnameRepository(IDbContextProvider<SPADbContext> dbContextProvider)
         : base(dbContextProvider)
        {
        }

        public async Task<PagedResultDto<PageBrandnameDto>> GetListBandname(ParamSearchBrandname input, int? tenantId)
        {
            try
            {
                string trangThais = string.Empty;
                if (input.TrangThais != null && input.TrangThais.Count > 0)
                {
                    trangThais = String.Join(",", input.TrangThais);
                }
                using var command = CreateCommand("spGetListBandname");
                command.Parameters.Add(new SqlParameter("@TenantId", tenantId ?? 1));
                command.Parameters.Add(new SqlParameter("@Keyword", input.Keyword ?? (object)DBNull.Value));
                command.Parameters.Add(new SqlParameter("@TrangThais", trangThais));
                command.Parameters.Add(new SqlParameter("@SkipCount", input.SkipCount));
                command.Parameters.Add(new SqlParameter("@MaxResultCount", input.MaxResultCount));
                command.Parameters.Add(new SqlParameter("@SortBy", input.SortBy));
                command.Parameters.Add(new SqlParameter("@SortType", input.SortType));

                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    string[] array = { "Data" };
                    var ds = new DataSet();
                    ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                    var ddd = ds.Tables;

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var data = ObjectHelper.FillCollection<PageBrandnameDto>(ds.Tables[0]);
                        return new PagedResultDto<PageBrandnameDto>()
                        {
                            TotalCount = Int32.Parse(ds.Tables[0].Rows[0]["TotalRow"].ToString()),
                            Items = data
                        };
                    }
                }
                return new PagedResultDto<PageBrandnameDto>();
            }
            catch (Exception)
            {
                return new PagedResultDto<PageBrandnameDto>();
            }
        }
        public async Task<PageBrandnameDto> GetInforBrandname_byId(Guid idBrandname)
        {
            using (var command = CreateCommand("spGetInforBrandname_byId"))
            {
                command.Parameters.Add(new SqlParameter("@IdBrandname", idBrandname));

                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    string[] array = { "Data" };
                    var ds = new DataSet();
                    ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                    var ddd = ds.Tables;

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var data = ObjectHelper.FillCollection<PageBrandnameDto>(ds.Tables[0]).FirstOrDefault();
                        return data;
                    }
                }
                return new PageBrandnameDto();
            }
        }
    }
}
