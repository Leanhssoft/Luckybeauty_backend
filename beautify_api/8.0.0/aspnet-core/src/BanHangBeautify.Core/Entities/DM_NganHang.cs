using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace BanHangBeautify.Entities
{
    public class DM_NganHang : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        [Required] public int TenantId { get; set; } = 1;
        [MaxLength(256)]
        public string MaNganHang { get; set; } = string.Empty;
        [Required, MaxLength(4000)]
        public string TenNganHang { get; set; } = string.Empty;
        public float? ChiPhiThanhToan { get; set; } = 0;
        public bool? TheoPhanTram { get; set; } = true;
        public bool? ThuPhiThanhToan { get; set; } = false;
        [MaxLength(50)]
        public string ChungTuApDung { get; set; } = string.Empty;
        [MaxLength(4000)]
        public string GhiChu { get; set; } = string.Empty;
        public int TrangThai { get; set; } = 1;
    }
}
