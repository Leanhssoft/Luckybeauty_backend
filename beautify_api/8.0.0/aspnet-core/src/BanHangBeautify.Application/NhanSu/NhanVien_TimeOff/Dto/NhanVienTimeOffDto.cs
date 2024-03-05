using System;

namespace BanHangBeautify.NhanSu.NhanVien_TimeOff.Dto
{
    public class NhanVienTimeOffDto
    {
        public Guid Id { get; set; }
        public string TenNhanVien { get; set; }
        public DateTime TuNgay { get; set; }
        public DateTime DenNgay { get; set; }
        public int LoaiNghi { get; set; }
        public double TongNgayNghi { get; set; }
    }
}
