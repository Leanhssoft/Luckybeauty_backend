using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using BanHangBeautify.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanHangBeautify.Data.Entities
{
    public class DM_PhongBan : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        [MaxLength(50)]
        public string MaPhongBan { set; get; }
        [MaxLength(256)]
        public string TenPhongBan { set; get; }
        public Guid IdChiNhanh { set; get; }
        [ForeignKey("IdChiNhanh")]
        public DM_ChiNhanh DM_ChiNhanh { get; set; }
        public DateTime NgayTao { set; get; }
        public Guid? NguoiTao { set; get; }
        public DateTime? NgaySua { set; get; }
        public Guid? NguoiSua { set; get; }
        public DateTime? NgayXoa { set; get; }
        public Guid? NguoiXoa { set; get; }
        public int TenantId { get; set; }
    }
}
