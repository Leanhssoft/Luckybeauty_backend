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
        public bool GioiTinh { get; set; }
        public string Email { get; set; }
        public string XungHo { get; set; }
        public string MoTa { get; set; }
        public int TrangThai { get; set; }
        public decimal TongTichDiem { get; set; }
        public string MaSoThue { get; set; }
        [MaxLength(2000)]
        public string Avatar { get; set; }
        public DateTime NgaySinh { get; set; }
        public int KieuNgaySinh { get; set; }
        public Guid IdLoaiKhach { get; set; }
        public DM_LoaiKhach LoaiKhach { get; set; }
        public Guid IdNhomKhach { get; set; }
        public DM_NhomKhachHang NhomKhach { get; set; }
        public Guid IdNguonKhach { get; set; }
        public Guid? IdTinhThanh { get; set; }
        public Guid? IdQuanHuyen { get; set; }
    }
}