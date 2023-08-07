using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.KhachHang.KhachHang.Dto
{
    public class KhachHangDetailDto
    {
        public Guid Id { get; set; }
        public string Avatar { get; set; }
        public string MaKhachHang { get; set; }
        public string TenKhachHang { get; set; }
        public string SoDienThoai { get; set; }
        public float DiemThuong { get; set; }
        public string MaSoThue { get; set; }
        public string Email { get; set; }
        public string DiaChi { get; set; }
        public string GioiTinh { get; set; }
        public string NgaySinh { get; set; }
        public string LoaiKhach { get; set; }
        public string NhomKhach { get; set; }
        public string NguonKhach { get; set; }
    }
}
