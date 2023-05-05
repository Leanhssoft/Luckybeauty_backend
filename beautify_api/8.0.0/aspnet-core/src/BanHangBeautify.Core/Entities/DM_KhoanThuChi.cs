using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BanHangBeautify.Entities
{
    public class DM_KhoanThuChi : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        [Required] public int TenantId { get; set; } = 1;
        [MaxLength(256)]
        public string MaKhoanThuChi { get; set; }
        [Required, MaxLength(4000)]
        public string TenKhoanThuChi { get; set; }
        public bool LaKhoanThu { get; set; } = true;
        [MaxLength(50)]
        public string ChungTuApDung { get; set; } = string.Empty;
        [MaxLength(4000)]
        public string GhiChu { get; set; } = string.Empty;
        public int TrangThai { get; set; } = 1;
    }
}
