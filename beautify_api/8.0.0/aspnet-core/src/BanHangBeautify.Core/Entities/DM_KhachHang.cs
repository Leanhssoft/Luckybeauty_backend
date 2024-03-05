using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanHangBeautify.Entities
{
    public class DM_KhachHang : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        [MaxLength(50)]
        public string MaKhachHang { get; set; }
        [MaxLength(256)]
        public string TenKhachHang { get; set; }
        [MaxLength(256)]
        public string TenKhachHang_KhongDau { get; set; }
        [MaxLength(256)]
        public string SoDienThoai { get; set; }
        [MaxLength(2000)]
        public string DiaChi { get; set; }
        public bool? GioiTinhNam { get; set; } = false;
        [MaxLength(2000)]
        public string Email { get; set; }
        [MaxLength(2000)]
        public string MoTa { get; set; }
        public int? TrangThai { get; set; } = 1;
        public float? TongTichDiem { get; set; } = 0;
        [MaxLength(256)]
        public string MaSoThue { get; set; }
        public string Avatar { get; set; }
        public DateTime? NgaySinh { get; set; }
        public int? KieuNgaySinh { get; set; } = 0;
        public int? IdLoaiKhach { get; set; } = 1;
        [ForeignKey("IdLoaiKhach")]
        public DM_LoaiKhach DM_LoaiKhach { get; set; }
        public Guid? IdNhomKhach { get; set; }
        [ForeignKey("IdNhomKhach")]
        public DM_NhomKhachHang DM_NhomKhachHang { get; set; }
        public Guid? IdNguonKhach { get; set; }
        [ForeignKey("IdNguonKhach")]
        public DM_NguonKhach DM_NguonKhach { get; set; }
        public Guid? IdTinhThanh { get; set; }
        [ForeignKey("IdTinhThanh")]
        public DM_TinhThanh DM_TinhThanh { get; set; }
        public Guid? IdQuanHuyen { get; set; }
        [ForeignKey("IdQuanHuyen")]
        public DM_QuanHuyen DM_QuanHuyen { get; set; }
        public Guid? IdKhachHangZOA { get; set; }
        [ForeignKey("IdKhachHangZOA")]
        public Zalo_KhachHangThanhVien Zalo_KhachHangThanhVien { get; set; }
    }
}
