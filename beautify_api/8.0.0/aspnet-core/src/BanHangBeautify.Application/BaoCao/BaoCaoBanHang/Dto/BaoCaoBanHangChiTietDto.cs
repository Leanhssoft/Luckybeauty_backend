using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.BaoCao.BaoCaoBanHang.Dto
{
    public class BaoCaoBanHangChiTietDto
    {
        public string MaHoaDon { get; set; }
        public DateTime NgayLapHoaDon { get; set; }
        public string TenKhachHang { get; set; }
        public string SoDienThoai { get; set; }
        public string NhomHangHoa { get; set; }
        public string TenHangHoa { get; set; }
        public decimal GiaBan { get; set; }
        public int SoLuong { get; set; }
        public decimal ThanhTien { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
