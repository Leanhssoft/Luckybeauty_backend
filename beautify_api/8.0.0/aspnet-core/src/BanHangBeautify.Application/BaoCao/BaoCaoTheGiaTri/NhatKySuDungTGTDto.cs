using BanHangBeautify.KhachHang.KhachHang.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.BaoCao.BaoCaoTheGiaTri
{
    public class NhatKySuDungTGTDto: CustomerBasicDto
    {
        public string SLoaiChungTu { get; set; }
        public byte? IdLoaiChungTu { get; set; }
        public string MaHoaDon { get; set; }
        public DateTime? NgayLapHoaDon { get; set; }
        public double? GtriDieuChinh { get; set; }
        public double? PhatSinhTang { get; set; }
        public double? PhatSinhGiam { get; set; }
    }
}
