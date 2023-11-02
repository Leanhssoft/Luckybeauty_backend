using System;

namespace BanHangBeautify.SMS.Dto
{
    public class CreateOrEditHeThongSMSDto
    {
        public Guid Id { set; get; }
        public Guid? IdChiNhanh { set; get; } = null;
        public Guid? IdNguoiGui { set; get; }
        public Guid? IdKhachHang { set; get; }
        public Guid? IdHoaDon { set; get; }
        public Guid? IdTinNhan { set; get; }
        public string SoDienThoai { set; get; }
        public int? SoTinGui { set; get; } = 1;
        public string NoiDungTin { set; get; }
        public DateTime? ThoiGianGui { set; get; } = DateTime.Now;
        public byte? IdLoaiTin { set; get; } = 1;
        public double? GiaTienMoiTinNhan { get; set; } = 1000;
        public int? TrangThai { get; set; } = 100;

        public string STrangThaiGuiTinNhan { set; get; }
        public string LoaiTin { set; get; }
        public string TenKhachHang { set; get; }
    }
}
