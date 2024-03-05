using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.KhachHang.KhachHang.Dto
{
    public class ImportExcelKhachHangDto
    {
        public string TenNhomKhachHang { get; set; }
        public int? IdLoaiKhach { get; set; } = 1;// 1.KH, 2.NCC
        public string TenKhachHang { get; set; }
        public string MaKhachHang { get; set; }
        public string SoDienThoai { get; set; }
        public DateTime? NgaySinh { get; set; }
        public bool? GioiTinhNam { get; set; }
        public string DiaChi { get; set; }
        public string MoTa { get; set; }
    }
}
