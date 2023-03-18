using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Entities
{
    public class NS_NhanVien_TimeOff:FullAuditedEntity<Guid>,IMustHaveTenant
    {
        public int TenantId { set; get; }
        public DateTime TuNgay { set; get; }
        public DateTime DenNgay { set; get; }
        public int TongNgayNghi { set; get; }
    }
}
