using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Zalo.DangKyThanhVien
{
    public class Zalo_KhachHangThanhVienDto
    {
        public Guid? Id { get; set; }
        public string TenDangKy { get; set; }
        public string SoDienThoaiDK { get; set; }
        public string ZOAUserId { get; set; }
        public string DiaChi { get; set; }
        public string TenTinhThanh { get; set; }
        public string TenQuanHuyen { get; set; }
    }
}
