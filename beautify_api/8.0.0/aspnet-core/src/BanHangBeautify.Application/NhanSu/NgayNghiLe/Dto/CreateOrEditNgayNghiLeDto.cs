using System;

namespace BanHangBeautify.NhanSu.NgayNghiLe.Dto
{
    public class CreateOrEditNgayNghiLeDto
    {
        public Guid Id { get; set; }
        public string TenNgayLe { get; set; }
        public DateTime TuNgay { get; set; }
        public DateTime DenNgay { get; set; }
    }
}
