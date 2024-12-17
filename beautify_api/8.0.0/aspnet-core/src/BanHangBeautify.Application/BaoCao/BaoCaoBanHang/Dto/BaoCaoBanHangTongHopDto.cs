using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.BaoCao.BaoCaoBanHang.Dto
{
    public class BaoCaoBanHang_SumFooterDto
    {
        public double? SumSoLuong { get; set; }
        public double? SumTienChietKhau { get; set; }
        public double? SumThanhTienTruocCK { get; set; }
        public double? SumThanhTienSauCK { get; set; }
        public double? SumGiaVon { get; set; }
        public double? SumLoiNhuan { get; set; }


    }
    public class BaoCaoBanHangTongHopDto : BaoCaoBanHang_SumFooterDto
    {
        public string TenHangHoa { set; get; }
        public string MaHangHoa { get; set; }
        public string TenNhomHang { get; set; }
        public double? SoLuong { get; set; }
        public double? TienChietKhau { get; set; }
        public double? ThanhTienTruocCK { get; set; }
        public double? ThanhTienSauCK { get; set; }
        public double? giaVon {  get; set; }
    }
}
