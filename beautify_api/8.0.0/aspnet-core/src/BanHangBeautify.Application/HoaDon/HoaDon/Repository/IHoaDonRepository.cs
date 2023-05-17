using BanHangBeautify.Checkin.Dto;
using BanHangBeautify.KhachHang.KhachHang.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.HoaDon.HoaDon.Repository
{
    public interface IHoaDonRepository
    {
        Task<string> GetMaHoaDon(int tenantId, Guid? idChiNhanh, int idLoaiChungTu, DateTime ngayLapHoaDon);
        Task<string> FnGetMaHoaDon(int tenantId, Guid? idChiNhanh, int idLoaiChungTu, DateTime? ngayLapHoaDon);
    }
}
