using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.KhachHang.KhachHang.Dto
{
    public class KhachHangThongTinTongHopDto
    {
        public int TongCuocHen { get; set; }
        public int CuocHenHoanThanh { get; set; }
        public int CuocHenHuy { get; set; }
        public double TongChiTieu { get; set; }
        public List<HoatDongKhachHang> HoatDongs { get; set; }
    }
    public class HoatDongKhachHang
    {
        public string HoatDong { get; set; }
        public DateTime ThoiGian { get; set; }
    }
}
