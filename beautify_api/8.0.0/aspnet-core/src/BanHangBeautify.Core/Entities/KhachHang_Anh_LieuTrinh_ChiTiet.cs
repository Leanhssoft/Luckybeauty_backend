using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Entities
{
    public class KhachHang_Anh_LieuTrinh_ChiTiet : FullAuditedEntity<Guid>
    {
        public Guid? AlbumId { set; get; }
        [ForeignKey("AlbumId")]
        public KhachHang_Anh_LieuTrinh KhachHang_Anh_LieuTrinh { get; set; }
        public int ImageIndex { get; set; }
        public string ImageUrl { get; set; }
    }
}
