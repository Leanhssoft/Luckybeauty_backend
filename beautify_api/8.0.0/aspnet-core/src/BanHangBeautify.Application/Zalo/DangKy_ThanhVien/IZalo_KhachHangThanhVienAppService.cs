using BanHangBeautify.Zalo.DangKyThanhVien;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Zalo.DangKy_ThanhVien
{
    public interface IZalo_KhachHangThanhVienAppService
    {
        Task<Zalo_KhachHangThanhVienDto> DangKyThanhVienZOA(Zalo_KhachHangThanhVienDto dto);
        Task<Zalo_KhachHangThanhVienDto> UpdateThanhVienZOA(Zalo_KhachHangThanhVienDto dto);
        Guid? GetId_fromZOAUserId(string zaloUserId);
    }
}
