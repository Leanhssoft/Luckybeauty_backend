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
        [ForeignKey("IdLoaiKhach")]
        public DM_LoaiKhach LoaiKhach { get; set; }
        public Guid IdNhomKhach { get; set; }
        [ForeignKey("IdNhomKhach")]
        public DM_NhomKhachHang NhomKhach { get; set; }
        public Guid IdNguonKhach { get; set; }
        [ForeignKey("IdNguonKhach")]
        public Guid? IdTinhThanh { get; set; }
        public Guid? IdQuanHuyen { get; set; }
        public DM_NguonKhach NguonKhach { get; set; }
        public Guid? NguoiTao { get; set; }
        public Guid? NguoiSua { get; set; }
        public DateTime NgayTao { get; set; }
        public DateTime? NgaySua { get; set; }
        public Guid? NguoiXoa { get; set; }
        public DateTime? NgayXoa { get; set; }
    }
}
