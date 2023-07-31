using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

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
        //[ForeignKey("IdLoaiKhach")]
        //public DM_LoaiKhach LoaiKhach { get; set; }
        public Guid? IdNhomKhach { get; set; }
        //[ForeignKey("IdNhomKhach")]
        //public DM_NhomKhachHang NhomKhach { get; set; }
        public Guid? IdNguonKhach { get; set; }
        //[ForeignKey("IdNguonKhach")]
        //public DM_NguonKhach NguonKhach { get; set; }
        public Guid? IdTinhThanh { get; set; }
        public Guid? IdQuanHuyen { get; set; }
        public Guid? NguoiTao { get; set; }
        public Guid? NguoiSua { get; set; }
        public Guid? NguoiXoa { get; set; }
    }
}
