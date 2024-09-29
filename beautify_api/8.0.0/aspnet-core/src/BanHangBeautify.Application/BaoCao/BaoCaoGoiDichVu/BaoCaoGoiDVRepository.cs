using Abp.Application.Services.Dto;
using Abp.EntityFrameworkCore;
using BanHangBeautify.AppCommon;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using BanHangBeautify.HoaDon.HoaDonChiTiet.Dto;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BanHangBeautify.AppCommon.CommonClass;

namespace BanHangBeautify.BaoCao.BaoCaoGoiDichVu
{
    public class BaoCaoGoiDVRepository : SPARepositoryBase, IBaoCaoGoiDVRepository
    {
        public BaoCaoGoiDVRepository(IDbContextProvider<SPADbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public async Task<PagedResultDto<ChiTietNhatKySuDungGDVDto>> BaoCaoSuDungGDV_ChiTiet(ParamSearch input)
        {
            string idChiNhanhs = string.Empty;
            if (input.IdChiNhanhs != null && input.IdChiNhanhs.Count > 0)
            {
                idChiNhanhs = string.Join(",", input.IdChiNhanhs);
            }
            using var command = CreateCommand("BaoCaoSuDungGDV_ChiTiet");
            command.Parameters.Add(new SqlParameter("@IdChiNhanhs", idChiNhanhs));
            command.Parameters.Add(new SqlParameter("@FromDate", input.FromDate ?? (object)DBNull.Value));// ngaylapHD sudung
            command.Parameters.Add(new SqlParameter("@ToDate", input.ToDate ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@TextSearch", input.TextSearch ?? string.Empty));// tim hoadon, khachhang
            command.Parameters.Add(new SqlParameter("@CurrentPage", input.CurrentPage ?? 0));
            command.Parameters.Add(new SqlParameter("@PageSize", input.PageSize ?? 10));

            using (var dataReader = await command.ExecuteReaderAsync())
            {
                string[] array = { "Data" };
                var ds = new DataSet();
                ds.Load(dataReader, LoadOption.OverwriteChanges, array);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var data = ObjectHelper.FillCollection<ChiTietNhatKySuDungGDVDto>(ds.Tables[0]);
                    return new PagedResultDto<ChiTietNhatKySuDungGDVDto>()
                    {
                        TotalCount = int.Parse(ds.Tables[0].Rows[0]["TotalRow"].ToString()),
                        Items = data,
                    };
                }
            }
            return new PagedResultDto<ChiTietNhatKySuDungGDVDto>();
        }
    }
}
