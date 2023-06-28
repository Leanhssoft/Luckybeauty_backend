using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace BanHangBeautify.Entities
{
    public class DM_NhomKhachHang : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        [MaxLength(50)]
        public string MaNhomKhach { get; set; }
        [MaxLength(256)]
        public string TenNhomKhach { get; set; }
        public float? GiamGia { get; set; } = 0;
        public bool? LaPhamTram { get; set; } = false;
        public bool? TuDongCapNhat { get; set; } = false;
        [MaxLength(2000)]
        public string MoTa { get; set; }
        public int? TrangThai { get; set; } = 1;
        public Guid? NguoiTao { get; set; }
        public Guid? NguoiSua { get; set; }
        public Guid? NguoiXoa { get; set; }
    }
}
