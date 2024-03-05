using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanHangBeautify.Entities
{
    public class NS_ChietKhauHoaDon : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public Guid? IdChiNhanh { get; set; }
        [ForeignKey("IdChiNhanh")]
        public DM_ChiNhanh DM_ChiNhanh { get; set; }
        public byte? LoaiChietKhau { get; set; } = 1; // 1. Theo % thực thu, 2. Theo % doanh thu, 3. Theo VNĐ
        public double? GiaTriChietKhau { get; set; } = 0;
        [MaxLength(50)]
        public string ChungTuApDung { get; set; }
        public string GhiChu { get; set; }
        public int TrangThai { get; set; } = 1;
    }
}
