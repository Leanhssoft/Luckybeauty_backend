using BanHangBeautify.HoaDon.HoaDon.Dto;
using BanHangBeautify.SPMigrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.HoaDon.HoaDonChiTiet.Dto
{
    public class ChiTietNhatKySuDungGDVDto
    {
        public string MaKhachHang { get; set; }
        public string TenKhachHang { get; set; }
        public string SoDienThoai { get; set; }

        public Guid? IdGoiDV { get; set; }
        public Guid? IdChiTietMua { get; set; }
        public string MaGoiDichVu { get; set; }
        public DateTime? NgayMuaGDV { get; set; }
        public string MaHangHoa { get; set; }
        public string TenHangHoa { get; set; }
        public double? SoLuongMua { get; set; }
        public double? DonGiaSauCK { get; set; }
        public double? ThanhTienSauCK { get; set; }

        public string MaHoaDonSD { get; set; }
        public DateTime? NgayLapHoaDonSD { get; set; }
        public double? SoLuongSD { get; set; }
        public double? GiaTriSuDung { get; set; } // soluongSD * dongia sauCK
        public string NVThucHiens { get; set; }
    }
}
