using System;
using System.Collections.Generic;

namespace BanHangBeautify.NhanSu.NhanVien_DichVu.Dto
{
    public class NhanVienDichVuDetailDto
    {
        public string TenNhanVien { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public float Rate { get; set; }
        public string ChucVu { get; set; }
        public List<DichVuNhanTheoNhanVienDto> DichVuThucHiens { get; set; } = new List<DichVuNhanTheoNhanVienDto>();
    }
    public class DichVuNhanTheoNhanVienDto
    {
        public Guid IdDichVu { get; set; }
        public string TenDichVu { get; set; }
        public string Image { get; set; }
        public decimal DonGia { get; set; }
        public string SoPhutThucHien { get; set; }
    }
}
