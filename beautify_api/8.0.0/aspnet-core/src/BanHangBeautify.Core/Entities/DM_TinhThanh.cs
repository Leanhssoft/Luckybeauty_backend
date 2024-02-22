using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace BanHangBeautify.Entities
{
    public class DM_TinhThanh : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        [MaxLength(10)]
        public string MaTinhThanh { get; set; }
        public string TenTinhThanh { get; set; }
    }
}
