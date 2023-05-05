using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.KhachHang.KhachHang.Dto
{
    public class KhachHangView
    {
        public Guid Id { get; set; }
        public string MaKhachHang { get; set; }
        public string TenKhachHang { get; set; }
        public string SoDienThoai { get; set; }
        public string TenNhomKhach { get; set; }
        public string GioiTinh { get; set; }
        public string NhanVienPhuTrach { get; set; }
        public decimal TongChiTieu { get; set; }
        public DateTime? CuocHenGanNhat { get; set; }
        public string TenNguonKhach { get; set; }
        public float? TongTichDiem { get; set; }
    }
}
