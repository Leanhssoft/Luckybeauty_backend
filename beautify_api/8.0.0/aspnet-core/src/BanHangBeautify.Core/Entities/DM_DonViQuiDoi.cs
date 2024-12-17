using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using BanHangBeautify.Data.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanHangBeautify.Entities
{
    public class DM_DonViQuiDoi : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        [MaxLength(50)]
        public string MaHangHoa { get; set; }
        [MaxLength(50)]
        public string TenDonViTinh { get; set; }
        public float? TyLeChuyenDoi { get; set; } = 1;
        public double? GiaBan { get; set; } = 0;
        public double? GiaVon { get; set; } = 0;

        public int? LaDonViTinhChuan { get; set; } = 1;
        public Guid IdHangHoa { get; set; }
        [ForeignKey("IdHangHoa")]
        public DM_HangHoa DM_HangHoa { get; set; }
    }
}
