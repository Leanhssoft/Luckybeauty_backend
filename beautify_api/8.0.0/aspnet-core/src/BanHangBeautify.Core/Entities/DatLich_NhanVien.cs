using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;

namespace BanHangBeautify.Entities
{
    public class DatLich_NhanVien : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public Guid? IdNhanVien { get; set; }
        // [ForeignKey("IdNhanVien")]
        //public NS_NhanVien? NS_NhanVien { get; set; }
        public Guid? IdDatLich { get; set; }
        //[ForeignKey("IdDatLich")]
        //public DatLich DatLich { get; set; }
    }
}
