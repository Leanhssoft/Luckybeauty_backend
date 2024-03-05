using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanHangBeautify.Entities
{
    public class DM_MauIn : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public Guid IdChiNhanh { get; set; }
        [ForeignKey("IdChiNhanh")]
        public DM_ChiNhanh DM_ChiNhanh { get; set; }
        public int LoaiChungTu { get; set; }
        [ForeignKey("LoaiChungTu")]
        public DM_LoaiChungTu DM_LoaiChungTu { get; set; }

        public string TenMauIn { get; set; }
        public bool LaMacDinh { get; set; }
        public string NoiDungMauIn { get; set; }
        public int TrangThai { get; set; }
    }
}
