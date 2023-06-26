using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.HoaDon.HoaDon.Dto
{
    public class PageHoaDonDto: CreateHoaDonDto
    {
        public string UserName { get; set; }// user lapphieu
        public string MaKhachHang { get; set; }
        public string TenKhachHang { get; set; }
        public string TenChiNhanh { get; set; }
        public string TxtTrangThaiHD { get; set; }
        public float? DaThanhToan { get; set; }
        public float? ConNo { get; set; }

        public float? SumTongTienHang { get; set; }
        public float? SumTongGiamGiaHD { get; set; }
        public float? SumTongThanhToan { get; set; }
        public float? SumDaThanhToan { get; set; }
    }
}
