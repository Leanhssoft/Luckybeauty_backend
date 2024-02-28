using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BanHangBeautify.AppCommon.CommonClass;

namespace BanHangBeautify.BaoCao.BaoCaoHoaHong
{
    public class ParamSearchBaoCaoHoaHong : ParamSearch
    {
        public HashSet<string> IdLoaiChungTus { get; set; }
        public HashSet<string> IdNhomHangs { get; set; }
    }
    public class PageBaoCaoHoaHongTongHopDto
    {
        public Guid? IdNhanVien { get; set; }
        public string MaNhanVien { get; set; }
        public string TenNhanVien { get; set; }
        public double? HoaHongThucHien_TienChietKhau { get; set; }
        public double? HoaHongTuVan_TienChietKhau { get; set; }
        public double? TongHoaHong { get; set; }
        public double? SumHoaHongThucHien { get; set; }
        public double? SumHoaHongTuVan { get; set; }
        public double? SumTongHoaHong { get; set; }
    }
    public class PageBaoCaoHoaHongChiTietDto : PageBaoCaoHoaHongTongHopDto
    {
        public Guid? IdHoaDonChiTiet { get; set; }
        public int? LoaiHoaDon { get; set; }
        public string MaHoaDon { get; set; }
        public DateTime? NgayLapHoaDon { get; set; }
        public string MaKhachHang { get; set; }
        public string TenKhachHang { get; set; }
        public string MaHangHoa { get; set; }
        public string TenHangHoa { get; set; }
        public string TenNhomHang { get; set; }
        public double? SoLuong { get; set; }
        public double? ThanhTienSauCK { get; set; }
        public double? HoaHongThucHien_PTChietKhau { get; set; }
        public double? HoaHongTuVan_PTChietKhau { get; set; }
        public double? SumSoLuong { get; set; }
        public double? SumThanhTienSauCK { get; set; }
    }
}
