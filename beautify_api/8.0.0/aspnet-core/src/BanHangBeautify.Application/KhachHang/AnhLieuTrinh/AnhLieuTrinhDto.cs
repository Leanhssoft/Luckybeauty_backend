using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.KhachHang.AnhLieuTrinh
{
    public class AnhLieuTrinhDto
    {
        public Guid Id { get; set; }
        public Guid? IdKhachHang { get; set; }
        public string AlbumName { get; set; }
        public DateTime? CreationTime { get; set; }
        public int? TongSoAnh { get; set; }
        public List<AnhLieuTrinh_ChiTietDto> LstAnhLieuTrinh { get; set; }
    }
    public class AnhLieuTrinh_ChiTietDto
    {
        public Guid? Id { get; set; }
        public Guid? AlbumId { get; set; }
        public int ImageIndex { get; set; }
        public string ImageUrl { get; set; }
    }
}
