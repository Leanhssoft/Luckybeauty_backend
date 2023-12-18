using System;

namespace BanHangBeautify.ChietKhau.ChietKhauDichVu.Dto
{
    public class ChietKhauDichVuItemDto
    {
        public Guid? Id { get; set; }
        public string TenDichVu { get; set; }
        public string TenNhomDichVu { get; set; }
        public double GiaTri { get; set; }
        public bool LaPhanTram { set; get; }
        public double? HoaHongThucHien { get; set; }
        public double? HoaHongYeuCauThucHien { get; set; }
        public double? HoaHongTuVan { get; set; }
        public double GiaDichVu { get; set; }
        public string TenNhanVien { get; set; }
    }
    
    public class ChietKhauDichVuItemDto_TachRiengCot
    {
        public Guid? IdNhanVien { get; set; }
        public Guid? IdDonViQuiDoi { get; set; }
        public double? GiaDichVu { get; set; }
        public string TenNhanVien { get; set; }
        public string TenDichVu { get; set; }
        public string TenNhomDichVu { get; set; }
        public double? HoaHongThucHien { get; set; }
        public double? HoaHongYeuCauThucHien { get; set; }
        public double? HoaHongTuVan { get; set; }
        public bool? LaPhanTram_HoaHongThucHien { get; set; }
        public bool? LaPhanTram_HoaHongYeuCauThucHien { get; set; }
        public bool? LaPhanTram_HoaHongTuVan { get; set; }
    }
}
