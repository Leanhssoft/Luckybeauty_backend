using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Entities
{
    public class SMS_Template : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public byte? IdLoaiTin { get; set; } = 0; // 0. Tin thường, 1. Sinh nhật, 2. Giao dịch, 3.Lịch hẹn
        public string TenMauTin { get; set; }
        public string NoiDungTinMau { get; set; }
        public bool? LaMacDinh { get; set; } = false;
        public byte? TrangThai { get; set; } = 1; // = tinyint sql (0-255), 1. Kích hoạt, 0.chưa kích hoạt
    }
}
