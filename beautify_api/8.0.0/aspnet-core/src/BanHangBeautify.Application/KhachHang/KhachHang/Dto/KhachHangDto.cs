using Abp.Application.Services.Dto;
using BanHangBeautify.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace BanHangBeautify.KhachHang.KhachHang.Dto
{
    public class KhachHangDto : EntityDto<Guid>

    {
        public int TenantId { set; get; }
        public string MaKhachHang { get; set; }
        public string TenKhachHang { get; set; }
        public string SoDienThoai { get; set; }
        public string DiaChi { get; set; }
        public bool? GioiTinhNam { get; set; } = false;
        public string Email { get; set; }
        public string XungHo { get; set; }
        public string MoTa { get; set; }
        public int TrangThai { get; set; }
        public float? TongTichDiem { get; set; } = 0;
        public string MaSoThue { get; set; }
        [MaxLength(2000)]
        public string Avatar { get; set; }
        public DateTime? NgaySinh { get; set; }
        public int? KieuNgaySinh { get; set; } = 0;
        public int IdLoaiKhach { get; set; } = 1;
        public DM_LoaiKhach LoaiKhach { get; set; }
        public Guid? IdNhomKhach { get; set; }
        public DM_NhomKhachHang NhomKhach { get; set; }
        public Guid? IdNguonKhach { get; set; }
        public Guid? IdTinhThanh { get; set; }
        public Guid? IdQuanHuyen { get; set; }
    }
}