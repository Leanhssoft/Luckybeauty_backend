using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace BanHangBeautify.Entities
{
    public class DM_NgayNghiLe : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { set; get; }
        [MaxLength(256)]
        public string TenNgayLe { set; get; }
        public DateTime TuNgay { set; get; }
        public DateTime DenNgay { set; get; }
        public int TongSoNgay { set; get; }
        public int TrangThai { set; get; }
    }
}
