using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace BanHangBeautify.Entities
{
    public class HT_CongTy : FullAuditedEntity<Guid>, IMustHaveTenant
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
        public string Website { set; get; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public string GhiChu { get; set; }
        public int TrangThai { get; set; }
        public DateTime NgayTao { get; set; }
    }
}
