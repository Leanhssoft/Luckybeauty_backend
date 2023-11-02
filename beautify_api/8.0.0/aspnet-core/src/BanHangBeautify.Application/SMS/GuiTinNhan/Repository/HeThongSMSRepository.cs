using Abp.Application.Services.Dto;
using Abp.EntityFrameworkCore;
using BanHangBeautify.Configuration.Common;
using BanHangBeautify.Entities;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using BanHangBeautify.KhachHang.KhachHang.Dto;
using BanHangBeautify.SMS.Dto;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BanHangBeautify.Configuration.Common.CommonClass;

namespace BanHangBeautify.SMS.GuiTinNhan.Repository
{
    public class HeThongSMSRepository : SPARepositoryBase<HeThong_SMS, Guid>, IHeThongSMSRepository
    {
        public HeThongSMSRepository(IDbContextProvider<SPADbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
        public async Task<PagedResultDto<CreateOrEditHeThongSMSDto>> GetListSMS(ParamSearch input)
        {
            if (input == null)
            {
                return new PagedResultDto<CreateOrEditHeThongSMSDto>();
            }
            string idChiNhanhs = string.Empty, trangThais = string.Empty;
            if (input.IdChiNhanhs != null && input.IdChiNhanhs.Count > 0)
            {
                idChiNhanhs = string.Join(",", input.IdChiNhanhs);
            }
            if (input.TrangThais != null && input.TrangThais.Count > 0)
            {
                trangThais = string.Join(",", input.TrangThais);
            }
            using var command = CreateCommand("spGetListSMS");
            command.Parameters.Add(new SqlParameter("@IdChiNhanhs", idChiNhanhs));
            command.Parameters.Add(new SqlParameter("@FromDate", input.FromDate ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@ToDate", input.ToDate ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@TrangThais", trangThais));
            command.Parameters.Add(new SqlParameter("@TextSearch", input.TextSearch ?? string.Empty));
            command.Parameters.Add(new SqlParameter("@CurrentPage", input.CurrentPage ?? 0));
            command.Parameters.Add(new SqlParameter("@PageSize", input.PageSize ?? 50));

            using (var dataReader = await command.ExecuteReaderAsync())
            {
                string[] array = { "Data"};
                var ds = new DataSet();
                ds.Load(dataReader, LoadOption.OverwriteChanges, array);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return new PagedResultDto<CreateOrEditHeThongSMSDto>()
                    {
                        TotalCount = int.Parse(ds.Tables[0].Rows[0]["TotalRow"].ToString()),
                        Items = ObjectHelper.FillCollection<CreateOrEditHeThongSMSDto>(ds.Tables[0])
                    };
                }
            }
            return new PagedResultDto<CreateOrEditHeThongSMSDto>();
        }
    }
}
