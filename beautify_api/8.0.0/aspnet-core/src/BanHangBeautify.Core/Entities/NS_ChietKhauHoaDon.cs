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
    public class NS_ChietKhauHoaDon : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public Guid Id { get; set; }
        public Guid IdChiNhanh { get; set; }
        public DM_ChiNhanh DM_ChiNhanh { get; set; }

        public byte LoaiChietKhau { get; set; }

        public decimal GiaTriChietKhau { get; set; }
        [MaxLength(50)]
        public string ChungTuApDung { get; set; }
        public int TrangThai { get; set; }
    }
}
