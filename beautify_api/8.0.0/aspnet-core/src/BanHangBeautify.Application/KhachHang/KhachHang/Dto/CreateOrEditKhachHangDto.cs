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
        public bool? GioiTinhNam { get; set; }
        [MaxLength(2000)]
        public string Email { get; set; }
        [MaxLength(2000)]
        public string MoTa { get; set; }
        public int TrangThai { get; set; }
        public float? TongTichDiem { get; set; } = 0;
        [MaxLength(256)]
        public string MaSoThue { get; set; }
        [MaxLength(2000)]
        public string Avatar { get; set; }
        public DateTime? NgaySinh { get; set; }
        public int? KieuNgaySinh { get; set; } = 0;
        public int IdLoaiKhach { get; set; } = 1;
        public Guid? IdNhomKhach { get; set; }
        public Guid? IdNguonKhach { get; set; }
        public Guid? IdTinhThanh { get; set; }
        public Guid? IdQuanHuyen { get; set; }
    }
}