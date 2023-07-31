using System;

namespace BanHangBeautify.NhanSu.NhanVien_TimeOff.Dto
{
    public class CreateOrEditNhanVienTimeOffDto
    {
        public Guid Id { get; set; }
        public DateTime TuNgay { get; set; }
        public DateTime DenNgay { get; set; }
        public int LoaiNghi { get; set; }
    }
}
