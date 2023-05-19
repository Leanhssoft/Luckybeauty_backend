using Abp.EntityFrameworkCore;
using BanHangBeautify.Checkin.Dto;
using BanHangBeautify.Common;
using BanHangBeautify.Entities;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using BanHangBeautify.HoaDon.HoaDon.Repository;
using BanHangBeautify.KhachHang.KhachHang.Dto;
using BanHangBeautify.KhachHang.KhachHang.Repository;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BanHangBeautify.Quy.DM_QuyHoaDon.Dto.Repository
{
    public class QuyHoaDonRepository : SPARepositoryBase<QuyHoaDon, Guid>, IQuyHoaDonRepository
    {
        public QuyHoaDonRepository(IDbContextProvider<SPADbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
        public async Task<string> FnGetMaPhieuThuChi(int tenantId, Guid? idChiNhanh, int idLoaiChungTu, DateTime? ngayLapHoaDon)
        {
            using var command = CreateCommand("select dbo.fnGetMaPhieuThuChi(@TenantId,@IdChiNhanh,@IdLoaiChungTu,@NgayLapHoaDon) ", System.Data.CommandType.Text);
            command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
            command.Parameters.Add(new SqlParameter("@IdChiNhanh", idChiNhanh ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@IdLoaiChungTu", idLoaiChungTu));
            command.Parameters.Add(new SqlParameter("@NgayLapHoaDon", ngayLapHoaDon ?? DateTime.Now));
            var code = (await command.ExecuteScalarAsync()).ToString();
            return code;
        }
    }
}
