using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BanHangBeautify.Entities
{
    public class DM_NhomKhachHang : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        [MaxLength(50)]
        public string MaNhom { get; set; }
        [MaxLength(256)]
        public string Tennhom { get; set; }
        [MaxLength(2000)]
        public string MoTa { get; set; }
        public int TrangThai { get; set; }
        public Guid? NguoiTao { get; set; }
        public Guid? NguoiSua { get; set; }
        public DateTime NgayTao { get; set; }
        public DateTime? NgaySua { get; set; }
        public Guid? NguoiXoa { get; set; }
        public DateTime? NgayXoa { get; set; }
    }
}
