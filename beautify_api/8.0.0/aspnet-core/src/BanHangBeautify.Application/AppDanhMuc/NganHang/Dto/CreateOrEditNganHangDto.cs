using System;

namespace BanHangBeautify.AppDanhMuc.NganHang.Dto
{
    public class CreateOrEditNganHangDto
    {
        public Guid Id { set; get; }
        public string MaNganHang { set; get; }
        public string TenNganHang { set; get; }
        public string BIN { get; set; }
        public string Logo { get; set; }
        public string TenRutGon { get; set; }
        public double ChiPhiThanhToan { set; get; }
        public bool TheoPhanTram { set; get; } = false;
        public bool ThuPhiThanhToan { set; get; } = false;
        public string ChungTuApDung { set; get; }
        public string GhiChu { set; get; }
        public int TrangThai { set; get; } = 1;
    }

    public class CreateOrEditNganHangManyDto
    {
        public string MaNganHang { set; get; }
        public string TenNganHang { set; get; }
        public string BIN { get; set; }
        public string Logo { get; set; }
        public string TenRutGon { get; set; }
        public double ChiPhiThanhToan { set; get; } = 0;
        public bool TheoPhanTram { set; get; } = false;
        public bool ThuPhiThanhToan { set; get; } = false;
        public string ChungTuApDung { set; get; }
        public string GhiChu { set; get; }
        public int TrangThai { set; get; } = 1;
    }
}
