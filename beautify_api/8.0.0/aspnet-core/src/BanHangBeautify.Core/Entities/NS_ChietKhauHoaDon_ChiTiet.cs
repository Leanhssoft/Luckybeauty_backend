using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using BanHangBeautify.Data.Entities;

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
