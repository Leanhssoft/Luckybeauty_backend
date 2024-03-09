using Abp.Application.Services.Dto;
using Abp.EntityFrameworkCore;
using BanHangBeautify.AppCommon;
using BanHangBeautify.BaoCao.BaoCaoBanHang.Dto;
using BanHangBeautify.Entities;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using BanHangBeautify.NhanSu.NhanVien.Dto;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.BaoCao.BaoCaoBanHang.Repository
{
    public class BaoCaoBanHangRepository : SPARepositoryBase<BH_HoaDon, Guid>, IBaoCaoBanHangRepository
    {
        public BaoCaoBanHangRepository(IDbContextProvider<SPADbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<PagedResultDto<BaoCaoBanHangChiTietDto>> GetBaoCaoBanHangChiTiet(ParamSearchBaoCaoBanHang input, int tenantId)
        {
            using var command = CreateCommand("prc_baoCao_BanHangChiTiet");
            string idChiNhanhs = string.Empty;
            if (input.IdChiNhanhs != null && input.IdChiNhanhs.Count > 0)
            {
                idChiNhanhs = string.Join(",", input.IdChiNhanhs);
            }
            command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
            command.Parameters.Add(new SqlParameter("@IdChiNhanhs", idChiNhanhs));
            command.Parameters.Add(new SqlParameter("@FromDate", input.FromDate ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@ToDate", input.ToDate ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@IdNhomHangHoa", input.IdNhomHangHoa ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@TextSearch", input.TextSearch ?? ""));
            command.Parameters.Add(new SqlParameter("@ColumnSort", input.ColumnSort ?? "ngayLapHoaDon"));
            command.Parameters.Add(new SqlParameter("@TypeSort", input.TypeSort ?? "desc"));
            command.Parameters.Add(new SqlParameter("@CurrentPage", input.CurrentPage ?? 1));
            command.Parameters.Add(new SqlParameter("@PageSize", input.PageSize ?? 10));

            using (var dataReader = await command.ExecuteReaderAsync())
            {
                string[] array = { "Data" };
                var ds = new DataSet();
                ds.Load(dataReader, LoadOption.OverwriteChanges, array);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var data = ObjectHelper.FillCollection<BaoCaoBanHangChiTietDto>(ds.Tables[0]);
                    return new PagedResultDto<BaoCaoBanHangChiTietDto>()
                    {
                        TotalCount = int.Parse(ds.Tables[0].Rows[0]["TotalCount"].ToString()),
                        Items = data
                    };
                }
            }
            return new PagedResultDto<BaoCaoBanHangChiTietDto>();
        }
        public async Task<PagedResultDto<BaoCaoBanHangTongHopDto>> GetBaoCaoBanHangTongHop(ParamSearchBaoCaoBanHang input, int tenantId)
        {
            using var command = CreateCommand("prc_baoCao_BanHangTongHop");
            string idChiNhanhs = string.Empty;
            if (input.IdChiNhanhs != null && input.IdChiNhanhs.Count > 0)
            {
                idChiNhanhs = string.Join(",", input.IdChiNhanhs);
            }
            command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
            command.Parameters.Add(new SqlParameter("@IdChiNhanhs", idChiNhanhs));
            command.Parameters.Add(new SqlParameter("@FromDate", input.FromDate ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@ToDate", input.ToDate ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@IdNhomHangHoa", input.IdNhomHangHoa ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@TextSearch", input.TextSearch ?? ""));
            command.Parameters.Add(new SqlParameter("@ColumnSort", input.ColumnSort ?? "tenNhomHang"));
            command.Parameters.Add(new SqlParameter("@TypeSort", input.TypeSort ?? "asc"));
            command.Parameters.Add(new SqlParameter("@CurrentPage", input.CurrentPage ?? 1));
            command.Parameters.Add(new SqlParameter("@PageSize", input.PageSize ?? 10));

            using (var dataReader = await command.ExecuteReaderAsync())
            {
                string[] array = { "Data" };
                var ds = new DataSet();
                ds.Load(dataReader, LoadOption.OverwriteChanges, array);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var data = ObjectHelper.FillCollection<BaoCaoBanHangTongHopDto>(ds.Tables[0]);
                    return new PagedResultDto<BaoCaoBanHangTongHopDto>()
                    {
                        TotalCount = int.Parse(ds.Tables[0].Rows[0]["TotalCount"].ToString()),
                        Items = data,
                    };
                }
            }
            return new PagedResultDto<BaoCaoBanHangTongHopDto>();
        }
    }
}
