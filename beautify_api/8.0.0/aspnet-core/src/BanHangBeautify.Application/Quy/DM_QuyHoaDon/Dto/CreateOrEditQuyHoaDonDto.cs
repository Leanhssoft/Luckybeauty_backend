using BanHangBeautify.Quy.QuyHoaDonChiTiet.Dto;
using System;
using System.Collections.Generic;

namespace BanHangBeautify.Quy.DM_QuyHoaDon.Dto
{
    public class CreateOrEditQuyHoaDonDto
    {
        public Guid Id { set; get; }
        public Guid? IdChiNhanh { set; get; }
        public int IdLoaiChungTu { set; get; }
        public string MaHoaDon { set; get; }
        public DateTime NgayLapHoaDon { set; get; }
        public double? TongTienThu { set; get; } = 0;
        public string NoiDungThu { set; get; }
        public bool HachToanKinhDoanh { set; get; }
        public bool? IsDelete { set; get; } = false;
        public int? TrangThai { set; get; } = 1;
        public List<QuyHoaDonChiTietDto> QuyHoaDon_ChiTiet { get; set; }

    }
}
