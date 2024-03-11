using Abp.Application.Services.Dto;
using Abp.EntityFrameworkCore;
using BanHangBeautify.AppCommon;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BanHangBeautify.BaoCao.BaoCaoHoaHong;

namespace BanHangBeautify.BaoCao.BaoCaoHoaHong
{
    public class BaoCaoHoaHongRepository : SPARepositoryBase, IBaoCaoHoaHongRepository
    {
        public BaoCaoHoaHongRepository(IDbContextProvider<SPADbContext> dbContextProvider) : base(dbContextProvider)
        {

        }
        public async Task<PagedResultDto<PageBaoCaoHoaHongTongHopDto>> BaoCaoHoaHongTongHop(ParamSearchBaoCaoHoaHong input)
        {
            string idChiNhanhs = string.Empty, idLoaiChungTus = string.Empty;
            if (input.IdChiNhanhs != null && input.IdChiNhanhs.Count > 0)
            {
                idChiNhanhs = string.Join(",", input.IdChiNhanhs);
            }
            if (input.IdLoaiChungTus != null && input.IdLoaiChungTus.Count > 0)
            {
                idLoaiChungTus = string.Join(",", input.IdLoaiChungTus);
            }
            using var command = CreateCommand("BaoCaoHoaHongTongHop");
            command.Parameters.Add(new SqlParameter("@IdChiNhanhs", idChiNhanhs));
            command.Parameters.Add(new SqlParameter("@IdLoaiChungTus", idLoaiChungTus));
            command.Parameters.Add(new SqlParameter("@FromDate", input.FromDate ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@ToDate", input.ToDate ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@TextSearch", input.TextSearch ?? string.Empty));
            command.Parameters.Add(new SqlParameter("@ColumnSort", input.ColumnSort ?? "TenNhanVien"));
            command.Parameters.Add(new SqlParameter("@TypeSort", input.TypeSort ?? "ASC"));
            command.Parameters.Add(new SqlParameter("@CurrentPage", input.CurrentPage ?? 0));
            command.Parameters.Add(new SqlParameter("@PageSize", input.PageSize ?? 10));


            using (var dataReader = await command.ExecuteReaderAsync())
            {
                string[] array = { "Data" };
                var ds = new DataSet();
                ds.Load(dataReader, LoadOption.OverwriteChanges, array);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var data = ObjectHelper.FillCollection<PageBaoCaoHoaHongTongHopDto>(ds.Tables[0]);
                    return new PagedResultDto<PageBaoCaoHoaHongTongHopDto>()
                    {
                        TotalCount = int.Parse(ds.Tables[0].Rows[0]["TotalRow"].ToString()),
                        Items = data,
                    };
                }
            }
            return new PagedResultDto<PageBaoCaoHoaHongTongHopDto>();
        }
        public async Task<PagedResultDto<PageBaoCaoHoaHongChiTietDto>> BaoCaoHoaHongChiTiet(ParamSearchBaoCaoHoaHong input)
        {
            string idChiNhanhs = string.Empty, idLoaiChungTus = string.Empty, idNhomHangs = string.Empty;
            if (input.IdChiNhanhs != null && input.IdChiNhanhs.Count > 0)
            {
                idChiNhanhs = string.Join(",", input.IdChiNhanhs);
            }
            if (input.IdNhomHangs != null && input.IdNhomHangs.Count > 0)
            {
                idNhomHangs = string.Join(",", input.IdNhomHangs);
            }
            if (input.IdLoaiChungTus != null && input.IdLoaiChungTus.Count > 0)
            {
                idLoaiChungTus = string.Join(",", input.IdLoaiChungTus);
            }
            using var command = CreateCommand("BaoCaoHoaHongChiTiet");
            command.Parameters.Add(new SqlParameter("@IdChiNhanhs", idChiNhanhs));
            command.Parameters.Add(new SqlParameter("@IdLoaiChungTus", idLoaiChungTus));
            command.Parameters.Add(new SqlParameter("@IdNhomHangs", idNhomHangs));
            command.Parameters.Add(new SqlParameter("@FromDate", input.FromDate ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@ToDate", input.ToDate ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@TextSearch", input.TextSearch ?? string.Empty));
            command.Parameters.Add(new SqlParameter("@ColumnSort", input.ColumnSort ?? "TenNhanVien"));
            command.Parameters.Add(new SqlParameter("@TypeSort", input.TypeSort ?? "asc"));
            command.Parameters.Add(new SqlParameter("@CurrentPage", input.CurrentPage ?? 0));
            command.Parameters.Add(new SqlParameter("@PageSize", input.PageSize ?? 10));

            using (var dataReader = await command.ExecuteReaderAsync())
            {
                string[] array = { "Data" };
                var ds = new DataSet();
                ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                var ddd = ds.Tables;

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var data = ObjectHelper.FillCollection<PageBaoCaoHoaHongChiTietDto>(ds.Tables[0]);
                    return new PagedResultDto<PageBaoCaoHoaHongChiTietDto>()
                    {
                        TotalCount = int.Parse(ds.Tables[0].Rows[0]["TotalRow"].ToString()),
                        Items = data
                    };
                }
            }
            return new PagedResultDto<PageBaoCaoHoaHongChiTietDto>();
        }
    }
}
