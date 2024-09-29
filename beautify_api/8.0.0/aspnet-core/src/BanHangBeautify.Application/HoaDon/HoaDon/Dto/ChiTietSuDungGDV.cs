using BanHangBeautify.HoaDon.HoaDonChiTiet.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.HoaDon.HoaDon.Dto
{
    public class ChiTietSuDungGDV: PageHoaDonChiTietDto
    {
        public string MaHoaDon { get; set; }
        public DateTime? NgayLapHoaDon { get; set; }
        public double? SoLuongMua { get; set; }
        public double? SoLuongDung { get; set; }
        public double? SoLuongConLai { get; set; }
    }
}
