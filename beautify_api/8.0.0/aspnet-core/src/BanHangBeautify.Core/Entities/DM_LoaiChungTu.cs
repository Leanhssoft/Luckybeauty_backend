using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanHangBeautify.Entities
{
    public class DM_LoaiChungTu : FullAuditedEntity<int>, IMustHaveTenant
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Castle.Components.DictionaryAdapter.Key("Id")]
        override
        public int Id
        { get; set; }
        public int TenantId { get; set; }
        [MaxLength(10)]
        public string MaLoaiChungTu { get; set; }
        [MaxLength(256)]
        public string TenLoaiChungTu { get; set; }
        public int TrangThai { get; set; }
        public Guid? NguoiTao { get; set; }
        public Guid? NguoiSua { get; set; }
        public Guid? NguoiXoa { get; set; }
    }
}
