using System;

namespace BanHangBeautify.HoaDon.HoaDonChiTiet.Dto
{
    public class PageHoaDonChiTietDto : HoaDonChiTietDto
    {
        public Guid? IdHangHoa { get; set; }
        public Guid? IdNhomHangHoa { get; set; }
        public double? GiaBan { get; set; }
        public string MaHangHoa { get; set; }
        public string TenHangHoa { get; set; }
        public string TenDonViTinh { get; set; }
        public string TenNhomHang { get; set; }
        public float? SoPhutThucHien { get; set; }
        public int IdLoaiHangHoa { get; set; }
        public string TenLoaiHangHoa { get; set; }
        public string TenNVThucHiens { get; set; }
    }
}
