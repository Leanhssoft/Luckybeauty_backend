using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;

namespace BanHangBeautify.KhachHang.KhachHang.Dto
{
    public class CreateOrEditKhachHangDto : EntityDto<Guid>
    {
        [MaxLength(50)]
        public string MaKhachHang { get; set; }
        [MaxLength(256)]
        public string TenKhachHang { get; set; }
        [MaxLength(256)]
        public string SoDienThoai { get; set; }
        [MaxLength(2000)]
        public string DiaChi { get; set; }
        public bool GioiTinh { get; set; }
        [MaxLength(2000)]
        public string Email { get; set; }
        [MaxLength(256)]
        public string XungHo { get; set; }
        [MaxLength(2000)]
        public string MoTa { get; set; }
        public int TrangThai { get; set; }
        public decimal TongTichDiem { get; set; }
        [MaxLength(256)]
        public string MaSoThue { get; set; }
        [MaxLength(2000)]
        public string Avatar { get; set; }
        public DateTime NgaySinh { get; set; }
        public int KieuNgaySinh { get; set; }
        public Guid IdLoaiKhach { get; set; }
        public Guid IdNhomKhach { get; set; }
        public Guid IdNguonKhach { get; set; }
        public Guid? IdTinhThanh { get; set; }
        public Guid? IdQuanHuyen { get; set; }
    }
}