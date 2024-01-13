using System;

namespace BanHangBeautify.KhachHang.KhachHang.Dto
{
    public class LichSuDatLichDto
    {
        public DateTime BookingDate { get; set; }
        public string TenHangHoa { get; set; }
        public string ThoiGianHen { get; set; } // start - end
        public double? GiaBan { get; set; }
        public string NVBook { get; set; }// khachhang chon khi booking
        public string NVThucHiens { get; set; }// nv thuc te lam
        public int TrangThai { get; set; }
        public string TxtTrangThai { get; set; }
    }
}
