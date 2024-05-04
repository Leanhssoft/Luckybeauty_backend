namespace BanHangBeautify.HoaDon.HoaDon.Dto
{
    public class PageHoaDonDto : CreateHoaDonDto
    {
        public string UserName { get; set; }// user lapphieu
        public string SoDienThoai { get; set; }
        public string MaKhachHang { get; set; }
        public string TenKhachHang { get; set; }
        public string Avatar { get; set; }
        public string TenChiNhanh { get; set; }
        public string TxtTrangThaiHD { get; set; }
        public double? DaThanhToan { get; set; }
        public double? ConNo { get; set; }

        public double? SumTongTienHang { get; set; }
        public double? SumTongGiamGiaHD { get; set; }
        public double? SumTongThanhToan { get; set; }
        public double? SumDaThanhToan { get; set; }
    }
}
