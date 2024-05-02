using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanHangBeautify.Entities
{
    public class KhachHang_Anh_LieuTrinh : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public Guid? IdKhachHang { set; get; }
        [ForeignKey("IdKhachHang")]
        public DM_KhachHang DM_KhachHang { get; set; }
        public string AlbumName { get; set; }
    }
}
