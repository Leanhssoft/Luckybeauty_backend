using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Zalo.DangKyThanhVien
{
    public class Zalo_KhachHangThanhVienDto
    {
        public Guid IdKhachHang { get; set; }
        public string TenDangKy { get; set; }
        public string SoDienThoaiDK { get; set; }
        public string ZOAUserId { get; set; }
    }
}
