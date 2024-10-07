using Abp.Application.Services.Dto;
using Abp.EntityFrameworkCore;
using BanHangBeautify.AppCommon;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using BanHangBeautify.HoaDon.HoaDon.Dto;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.BaoCao.BaoCaoTheGiaTri
{
    public class BaoCaoTGTRepository: SPARepositoryBase, IBaoCaoTGTRepository

    {
        public BaoCaoTGTRepository(IDbContextProvider<SPADbContext> dbContextProvider) : base(dbContextProvider)
        {

        }
        public async Task<PagedResultDto<NhatKySuDungTGTDto>> GetNhatKySuDungTGT_ChiTiet(ParamSearchNhatKyGDV param)
        {
            string idChiNhanhs = string.Empty, loaiChungTus = string.Empty;
            if (param.IdChiNhanhs != null && param.IdChiNhanhs.Count > 0)
            {
                idChiNhanhs = string.Join(",", param.IdChiNhanhs);
            }
            if (param.IdLoaiChungTus != null && param.IdLoaiChungTus.Count > 0)
            {
                loaiChungTus = string.Join(",", param.IdLoaiChungTus);
            }
            using var command = CreateCommand("GetNhatKySuDungTGT_ChiTiet");
            command.Parameters.Add(new SqlParameter("@IdChiNhanhs", idChiNhanhs));
            command.Parameters.Add(new SqlParameter("@IdKhachHang", param?.IdCustomer ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@IdLoaiChungTus", loaiChungTus));
            command.Parameters.Add(new SqlParameter("@FromDate", param?.FromDate ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@ToDate", param?.ToDate ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@TextSearch", param?.TextSearch ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@CurrentPage", param?.CurrentPage ?? 1));
            command.Parameters.Add(new SqlParameter("@PageSize", param?.PageSize ?? 10));

            using (var dataReader = await command.ExecuteReaderAsync())
            {
                string[] array = { "Data" };
                var ds = new DataSet();
                ds.Load(dataReader, LoadOption.OverwriteChanges, array);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var data = ObjectHelper.FillCollection<NhatKySuDungTGTDto>(ds.Tables[0]);
                    return new PagedResultDto<NhatKySuDungTGTDto>()
                    {
                        TotalCount = Int32.Parse(ds.Tables[0].Rows[0]["TotalRow"].ToString()),
                        Items = data
                    };
                }
            }
            return new PagedResultDto<NhatKySuDungTGTDto>();
        }

    }
}
