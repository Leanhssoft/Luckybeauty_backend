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
    public class DM_KhuyenMai : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public string MaKhuyenMai { get; set; }
        public string TenKhuyenMai { get; set; }
        public byte LoaiKhuyenMai { get; set; }
        public byte HinhThucKM { get; set; }
        public DateTime ThoiGianApDung { get; set; }
        public DateTime? ThoiGianKetThuc { get; set; }
        public bool TatCaKhachHang { get; set; }
        public bool TatCaChiNhanh { get; set; }
        public bool TatCaNhanVien { get; set; }
        [MaxLength(256)]
        public string NgayApDung { get; set; }
        [MaxLength(256)]
        public string ThangApDung { get; set; }
        [MaxLength(256)]
        public string ThuApDung { get; set; }
        [MaxLength(256)]
        public string GioApDung { get; set; }
        [MaxLength(2000)]
        public string GhiChu { get; set; }
        public int TrangThai { get; set; }

    }
}
