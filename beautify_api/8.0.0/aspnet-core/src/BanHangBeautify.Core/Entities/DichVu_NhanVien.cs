using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using BanHangBeautify.Data.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanHangBeautify.Entities
{
    public class DichVu_NhanVien : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public Guid IdDonViQuyDoi { get; set; }
        [ForeignKey("IdDonViQuyDoi")]
        public DM_DonViQuiDoi DM_DonViQuiDoi { get; set; }
        public Guid IdNhanVien { get; set; }
        [ForeignKey("IdNhanVien")]
        public NS_NhanVien NS_NhanVien { get; set; }
    }
}
