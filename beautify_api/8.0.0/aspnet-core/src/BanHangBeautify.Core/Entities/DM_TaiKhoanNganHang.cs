using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanHangBeautify.Entities
{
    public class DM_TaiKhoanNganHang : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        [Required] public int TenantId { get; set; } = 1;
        [MaxLength(256)]
        [Required]
        public Guid IdNganHang { get; set; }
        [ForeignKey("IdNganHang")]
        public DM_NganHang DM_NganHang { get; set; }
        [Required, MaxLength(256)]
        public string SoTaiKhoan { get; set; } = string.Empty;
        [Required, MaxLength(500)]
        public string TenChuThe { get; set; } = string.Empty;
        [MaxLength(4000)]
        public string GhiChu { get; set; } = string.Empty;
        public int TrangThai { get; set; } = 1;
        public Guid? IdChiNhanh { get; set; }
        [ForeignKey("IdChiNhanh")]
        public DM_ChiNhanh DM_ChiNhanh { get; set; }
        public bool IsDefault { get; set; }

    }
}
