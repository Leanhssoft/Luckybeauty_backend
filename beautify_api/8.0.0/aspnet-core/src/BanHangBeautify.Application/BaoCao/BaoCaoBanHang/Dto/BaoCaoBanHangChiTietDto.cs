using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.BaoCao.BaoCaoBanHang.Dto
{
    public class BaoCaoBanHangChiTietDto : BaoCaoBanHang_SumFooterDto
    {
        public string MaHoaDon { get; set; }
        public DateTime NgayLapHoaDon { get; set; }
        public string TenKhachHang { get; set; }
        public string SoDienThoai { get; set; }
        public string TenNhomHang { get; set; }
        public string MaHangHoa { get; set; }
        public string TenHangHoa { get; set; }
        public double SoLuong { get; set; }
        public double? DonGiaTruocCK { get; set; }
        public double? ThanhTienTruocCK { get; set; }
        public double? TienChietKhau { get; set; }
        public double? ThanhTienSauCK { get; set; }
        public double? giaVon { get; set; }

    }
}
