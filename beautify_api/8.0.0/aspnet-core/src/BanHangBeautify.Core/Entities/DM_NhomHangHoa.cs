using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace BanHangBeautify.Entities
{
    public class DM_NhomHangHoa : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        [MaxLength(50)]
        public string MaNhomHang { get; set; }
        [MaxLength(256)]
        public string TenNhomHang { get; set; }
        [MaxLength(256)]
        public string TenNhomHang_KhongDau { get; set; }
        public bool? LaNhomHangHoa { get; set; } = false;
        public Guid? IdParent { get; set; }
        public string Color { get; set; }
        public string MoTa { get; set; }
        public byte? ThuTuHienThi { get; set; }
    }
}
