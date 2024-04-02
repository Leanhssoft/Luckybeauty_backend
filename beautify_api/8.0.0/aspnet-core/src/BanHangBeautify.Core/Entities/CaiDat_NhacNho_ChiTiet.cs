using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Entities
{
    public class CaiDat_NhacNho_ChiTiet : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public Guid IdCaiDatNhacNho { get; set; }
        [ForeignKey("IdCaiDatNhacNho")]
        public SMS_CaiDat_NhacNho SMS_CaiDat_NhacNho { get; set; }
        public byte? HinhThucGui { get; set; } = 0;// 1. SMS, 2.Zalo, 3.Email
        public byte? TrangThai { get; set; } = 0;// 1.on, 0.off
    }
}
