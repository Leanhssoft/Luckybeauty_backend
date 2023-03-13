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
    public  class HT_CongTy : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        [MaxLength(256)]
        public string TenCongTy { get; set; }
        [MaxLength(256)]
        public string SoDienThoai { get; set; }
        [MaxLength(2000)]
        public string DiaChi { get; set; }
        [MaxLength(256)]
        public string MaSoThue { get; set; }
        [MaxLength(2000)]
        public string Logo { get; set; }
        [MaxLength(2000)]
        public string GhiChu { get; set; }
        public int TrangThai { get; set; }
        public DateTime NgayTao { get; set; }
    }
}
