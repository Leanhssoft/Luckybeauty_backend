﻿namespace BanHangBeautify.Quy.DM_QuyHoaDon.Dto
{
    public class QuyHoaDonViewItemDto : QuyHoaDonDto
    {
        public string LoaiPhieu { get; set; }// 11.thu/12.chi
        public string SHinhThucThanhToan { get; set; }
        public string MaNguoiNop { get; set; }
        public string TenNguoiNop { get; set; }
        public string TxtTrangThai { get; set; }
        public string TenKhoanThuChi { get; set; }
        public int? TrangThai { get; set; } = 1;
    }
}
