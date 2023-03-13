using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BanHangBeautify.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanHangBeautify.Entities
{
    public class DichVu_NhanVien : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public Guid IdHangHoa { get; set; }
        [ForeignKey("IdHangHoa")]
        public DM_HangHoa DM_HangHoa { get; set; }
        public Guid IdNhanVien { get; set; }
        [ForeignKey("IdNhanVien")]
        public NS_NhanVien NS_NhanVien { get; set; }
    }
}
