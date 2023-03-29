using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
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
        public string Color { get; set; }
        public string MoTa { get; set; }
        public Guid? NguoiTao { get; set; }
        public Guid? NguoiSua { get; set; }
        public Guid? NguoiXoa { get; set; }
    }
}
