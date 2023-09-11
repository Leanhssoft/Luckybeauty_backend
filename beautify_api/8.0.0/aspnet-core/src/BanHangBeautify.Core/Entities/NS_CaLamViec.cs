using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanHangBeautify.Entities
{
    public class NS_CaLamViec : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { set; get; }
        public Guid IdChiNhanh { set; get; }
        [ForeignKey("IdChiNhanh")]
        public DM_ChiNhanh DM_ChiNhanh { set; get; }
        [MaxLength(50)]
        public string MaCa { set; get; }
        [MaxLength(256)]
        public string TenCa { set; get; }
        public DateTime GioVao { set; get; }
        public DateTime GioRa { set; get; }
        public bool LaNghiGiuaCa { set; get; }
        public DateTime? GioNghiTu { set; get; }
        public DateTime? GioNghiDen { set; get; }
        public float TongGioCong { set; get; }
        public int TrangThai { set; get; }
    }
}
