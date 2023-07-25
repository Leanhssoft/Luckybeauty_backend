using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using BanHangBeautify.Data.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanHangBeautify.Entities
{
    public class NS_ChietKhauHoaDon_ChiTiet : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public Guid Id { get; set; }

        public Guid IdChietKhauHD { get; set; }
        [ForeignKey("IdChietKhauHD")]
        public NS_ChietKhauHoaDon NS_ChietKhauHoaDon { get; set; }
        public Guid IdNhanVien { get; set; }
        [ForeignKey("IdNhanVien")]
        public NS_NhanVien NS_NhanVien { get; set; }
    }
}
