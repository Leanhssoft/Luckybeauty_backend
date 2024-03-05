using System;

namespace BanHangBeautify.SMS.Dto
{
    public class CreateOrEditHeThongSMSDto
    {
        public Guid Id { set; get; }
        public Guid? IdChiNhanh { set; get; } = null;
        public long? IdNguoiGui { set; get; }
        public Guid? IdKhachHang { set; get; }
        public Guid? IdHoaDon { set; get; }
        public string IdTinNhan { set; get; } // a4e3472e-5ff8-4a03-beb6-38b3393a42b5186 (gần giống Guid nhưng không phải - vì chuỗi cuối cùng gồm 15 kí tự)
        public string SoDienThoai { set; get; }
        public int? SoTinGui { set; get; } = 1;
        public string NoiDungTin { set; get; }
        public DateTime? ThoiGianGui { set; get; } = DateTime.Now;
        public byte? IdLoaiTin { set; get; } = 1;
        public double? GiaTienMoiTinNhan { get; set; } = 950;// hiện tại, đang mặc định giá này cho all nhà mạng (nếu sau cần thì thêm data)
        public int? TrangThai { get; set; } = 100;
        public byte? HinhThucGui { get; set; } = 0;// 1.sms, 2.zalo, 3.gmail

        public string STrangThaiGuiTinNhan { set; get; }
        public string LoaiTin { set; get; }
        public string TenKhachHang { set; get; }
    }
}
