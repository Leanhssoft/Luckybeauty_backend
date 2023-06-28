using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Entities
{
    public class HT_CauHinhPhanMem:FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public Guid IdChiNhanh { get; set; }
        public bool TichDiem { get; set; }
        public bool KhuyenMai { get; set; }
        public bool MauInMacDinh { get; set; }
        public bool SuDungMaChungTu { get; set; }
        public bool QLKhachHangTheoChiNhanh { get; set; }
    }
}
