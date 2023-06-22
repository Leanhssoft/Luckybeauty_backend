using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.HoaDon.HoaDonChiTiet.Dto
{
    public class PageHoaDonChiTietDto:HoaDonChiTietDto
    {
        public Guid? IdHangHoa { get; set; }
        public Guid? IdNhomHangHoa { get; set; }
        public float? GiaBan { get; set; }
        public string MaHangHoa { get; set; }
        public string TenHangHoa { get; set; }
        public string TenNhomHang { get; set; }
        public float? SoPhutThucHien { get; set; }
        public int IdLoaiHangHoa { get; set; }
        public string TenLoaiHangHoa { get; set; }
    }
}
