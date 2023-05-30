using System;
using System.ComponentModel.DataAnnotations;

namespace BanHangBeautify.AppDanhMuc.AppChiNhanh.Dto
{
    public class ChiNhanhDto
    {
        public Guid Id { set;get; }
        public Guid IdCongTy { get; set; }
        public string MaChiNhanh { get; set; }
        public string TenChiNhanh { get; set; }
        public string SoDienThoai { get; set; }
        public string DiaChi { get; set; }
        public string MaSoThue { get; set; }
        public string Logo { get; set; }
        public string GhiChu { get; set; }
        public int TrangThai { get; set; }
        public DateTime NgayHetHan { get; set; }
        public DateTime NgayApDung { get; set; }
    }
}
