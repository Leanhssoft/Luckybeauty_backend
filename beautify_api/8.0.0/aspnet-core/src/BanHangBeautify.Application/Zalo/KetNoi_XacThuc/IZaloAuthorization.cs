using BanHangBeautify.Zalo.DangKyThanhVien;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Zalo.KetNoi_XacThuc
{
    public interface IZaloAuthorization
    {
        Task<ZaloAuthorizationDto> Innit_orGetToken();
        Task<Zalo_KhachHangThanhVienDto> GetInforUser_ofOA(string accessToken, string userId);
    }
}
