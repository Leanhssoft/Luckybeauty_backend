using System;

namespace BanHangBeautify.AppDanhMuc.NganHang.Dto
{
    public class NganHangDto
    {
        public Guid Id { set; get; }
        public string MaNganHang { set; get; }
        public string TenNganHang { set; get; }
        public string TenRutGon { set; get; }
        public string Bin { set; get; }// mã pin theo VietQR
        public double? ChiPhiThanhToan { set; get; } = 0;
        public bool? TheoPhanTram { set; get; } = true;
        public bool? ThuPhiThanhToan { set; get; } = false;
        public string ChungTuApDung { set; get; }
        public string GhiChu { set; get; }
        public string Logo { set; get; }
        public int TrangThai { set; get; }
    }
}
