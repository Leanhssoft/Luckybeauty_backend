using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using BanHangBeautify.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanHangBeautify.Data.Entities
{
    public class DatLich_ChiTiet : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public Guid IdDatLich { set; get; }
        [ForeignKey("IdDatLich")]
        public DatLich DatLich { get; set; }
        public Guid IdDonViQuiDoi { get; set; }
        [ForeignKey("IdDonViQuiDoi")]
        public DM_DonViQuiDoi DM_DonViQuiDoi { get; set; }
        public Guid? IdNhanVien { get; set; }
        //[ForeignKey("IdNhanVien")]
        //public NS_NhanVien NS_NhanVien { get; set; }
        public int TenantId { get; set; }
    }
}
