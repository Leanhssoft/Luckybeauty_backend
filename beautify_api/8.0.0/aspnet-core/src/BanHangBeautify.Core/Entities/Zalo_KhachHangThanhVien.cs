using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Entities
{
    public class Zalo_KhachHangThanhVien: FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public DM_KhachHang DM_KhachHang { get; set; }
        [MaxLength(500)]
        public string TenDangKy { get; set; }
        [MaxLength(100)]
        public string SoDienThoaiDK { get; set; } 
        public string DiaChi { get; set; } 
        public string TenTinhThanh { get; set; } 
        public string TenQuanHuyen { get; set; } 
        public string ZOAUserId { get; set; }
    }
}
