using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanHangBeautify.Data.Entities
{
    public class DM_LoaiHangHoa : FullAuditedEntity<int>, IMustHaveTenant
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Castle.Components.DictionaryAdapter.Key("Id")]
        override
        public int Id { get; set; }
        public string MaLoai { get; set; }
        public string TenLoai { get; set; }
        public int TenantId { get; set; }
        public int TrangThai { get; set; } = 1;
        public Guid? NguoiTao { get; set; }
        public Guid? NguoiSua { get; set; }
        public Guid? NguoiXoa { get; set; }
    }
}
