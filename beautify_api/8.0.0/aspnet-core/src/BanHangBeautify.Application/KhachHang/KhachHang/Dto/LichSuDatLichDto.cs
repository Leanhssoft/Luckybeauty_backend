using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.KhachHang.KhachHang.Dto
{
    public class LichSuDatLichDto
    {
        public DateTime BookingDate { get; set; }
        public string TenDichVu { get; set; }
        public float ThoiGianThucHien { get; set; }
        public string ThoiGianBatDau { get; set; }
        public decimal DonGia { get; set; }
        public string NhanVienThucHien { get; set; }
        public string TrangThai { get; set; }
    }
}
