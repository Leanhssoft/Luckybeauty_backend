using System;

namespace BanHangBeautify.Quy.DM_QuyHoaDon.Dto
{
    public class GetAllQuyHoaDonItemDto
    {
        public Guid Id { set; get; }
        public Guid? IdChiNhanh { set; get; }
        public Guid? IdHoaDonLienQuan { set; get; } // get để check bên ngoài giao diện (phiếu nào thu/chi từ hóa đơn)
        public string LoaiPhieu { get; set; }// 11.thu/12.chi
        public string MaHoaDon { get; set; }
        public DateTime? NgayLapHoaDon { get; set; }
        public double TongTienThu { get; set; }
        public string TenNguoiNop { get; set; }
        public DateTime CreationTime { get; set; }
        public string TenKhoanThuChi { get; set; }
        public string HinhThucThanhToan { get; set; }//1,2,3
        public string SHinhThucThanhToan { get; set; }// mat, pos, ck
        public string TxtTrangThai { get; set; }
        public int? TrangThai { get; set; }
    }
}
