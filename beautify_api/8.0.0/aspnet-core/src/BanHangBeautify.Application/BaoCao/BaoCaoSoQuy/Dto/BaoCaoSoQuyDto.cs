using BanHangBeautify.HoaDon.HoaDon.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BanHangBeautify.AppCommon.CommonClass;

namespace BanHangBeautify.BaoCao.BaoCaoSoQuy.Dto
{
    public class BaoCaoSoQuyDto
    {
        public Guid Id { get; set; }
        public string MaHoaDon { get; set; }
        public DateTime NgayLapHoaDon { get; set; }
        public decimal TienThu { get; set; }
        public decimal TienChi { get; set; }
        public decimal TonLuyKe { get; set; }
        public string NguoiNop { get; set; }
        public string GhiChu { get; set; }
    }

    public class ParamSearchBaoCaoTaiChinh : ParamSearch
    {
        public HashSet<string> IdLoaiChungTus { get; set; }
        public string TextSearchDichVu { get; set; }
        public DateTime? NgayLapHoaDon_FromDate { get; set; }
        public DateTime? NgayLapHoaDon_ToDate { get; set; }
    }

    public class BaoCaoTaiChinh_ChiTietSoQuyDto
    {
        public Guid Id { get; set; }
        public string MaPhieuThuChi { get; set; }
        public DateTime? NgayLapPhieu { get; set; }
        public string MaNguoiNopTien { get; set; }
        public string TenNguoiNopTien { get; set; }
        public string MaHoaDonLienQuans { get; set; }
        public double? Thu_TienMat { get; set; }
        public double? Thu_TienChuyenKhoan { get; set; }
        public double? Thu_TienQuyetThe { get; set; }
        public double? Chi_TienMat { get; set; }
        public double? Chi_TienChuyenKhoan { get; set; }
        public double? Chi_TienQuyetThe { get; set; }
        public double? TienThu { get; set; }
        public double? TienChi { get; set; }
        public double? TongThuChi { get; set; }
        public string NoiDungThu { get; set; }

        public double? Sum_ThuTienMat { get; set; }
        public double? Sum_ThuTienChuyenKhoan { get; set; }
        public double? Sum_ThuTienQuyetThe { get; set; }
        public double? Sum_ChiTienMat { get; set; }
        public double? Sum_ChiTienChuyenKhoan { get; set; }
        public double? Sum_ChiTienQuyetThe { get; set; }
        public double? SumTienThu { get; set; }
        public double? SumTienChi { get; set; }
        public double? SumTongThuChi { get; set; }
    }
    public class BaoCaoChiTietCongNoDto
    {
        public int? IdLoaiKhach { get; set; }
        public int? IdLoaiChungTu { get; set; }
        public string MaKhachHang { get; set; }
        public string TenKhachHang { get; set; }
        public string MaHoaDon { get; set; }
        public DateTime? NgayLapHoaDon { get; set; }
        public double? TongThanhToan { get; set; }
        public double? KhachDaTra { get; set; }
        public double? ConNo { get; set; }
        public double? GhiChuHD { get; set; }

        public string TenHangHoa { get; set; }
        public double? SoLuong { get; set; }
        public double? DonGiaSauVAT { get; set; }
        public double? ThanhTienSauVAT { get; set; }

        public double? SumTongThanhToan { get; set; }
        public double? SumKhachDaTra { get; set; }
        public double? SumConNo { get; set; }
        public double? SumSoLuong { get; set; }
        public double? SumThanhTienSauVAT { get; set; }
    }
}
