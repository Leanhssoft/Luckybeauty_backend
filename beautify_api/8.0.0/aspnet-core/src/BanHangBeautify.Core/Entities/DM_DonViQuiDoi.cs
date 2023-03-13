using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using BanHangBeautify.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanHangBeautify.Entities
{
    public class DM_DonViQuiDoi : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        [MaxLength(50)]
        public string MaHangHoa { get; set; }
        [MaxLength(50)]
        public string TenDonVi { get; set; }
        public decimal TyLeChuyenDoi { get; set; }
        public decimal GiaBan { get; set; }
        public int LaDonViTinhChuan { get; set; }
        public Guid IdHangHoa { get; set; }
        [ForeignKey("IdHangHoa")]
        public DM_HangHoa DM_HangHoa { get; set; }
    }
}
