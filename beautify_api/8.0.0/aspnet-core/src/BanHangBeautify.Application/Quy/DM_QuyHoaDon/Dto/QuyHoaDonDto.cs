using System;

namespace BanHangBeautify.Quy.DM_QuyHoaDon.Dto
{
    public class QuyHoaDonDto
    {
        public Guid Id { set; get; }
        public Guid? IdChiNhanh { set; get; }
        public int IdLoaiChungTu { set; get; }
        public string MaHoaDon { set; get; }
        public DateTime NgayLapHoaDon { set; get; }
        public double? TongTienThu { set; get; } = 0;
        public string NoiDungThu { set; get; }
        public bool? HachToanKinhDoanh { set; get; } = true;
    } 
    public class ThuChi_DauKyCuoiKyDto
    {
        public double? TonDauKy { set; get; } = 0;
        public double? ThuTrongKy { set; get; } = 0;
        public double? ChiTrongKy { set; get; } = 0;
        public double? TonCuoiKy { set; get; } = 0;
    }
}
