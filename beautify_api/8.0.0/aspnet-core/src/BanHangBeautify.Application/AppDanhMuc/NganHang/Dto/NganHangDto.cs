using System;

namespace BanHangBeautify.AppDanhMuc.NganHang.Dto
{
    public class NganHangDto
    {
        public Guid Id { set; get; }
        public string MaNganHang { set; get; }
        public string TenNganHang { set; get; }
        public double ChiPhiThanhToan { set; get; }
        public bool TheoPhanTram { set; get; }
        public bool ThuPhiThanhToan { set; get; }
        public string ChungTuApDung { set; get; }
        public string GhiChu { set; get; }
        public int TrangThai { set; get; }
    }
}
