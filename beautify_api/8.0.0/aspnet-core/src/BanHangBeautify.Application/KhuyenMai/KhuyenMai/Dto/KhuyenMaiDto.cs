using System;
using System.Collections.Generic;

namespace BanHangBeautify.KhuyenMai.KhuyenMai.Dto
{
    public class KhuyenMaiDto
    {
        public Guid Id { get; set; }
        public string MaKhuyenMai { get; set; }
        public string TenKhuyenMai { get; set; }
        public string LoaiKhuyenMai { get; set; }
        public string HinhThucKM { set; get; }
        public DateTime ThoiGianApDung { get; set; }
        public DateTime? ThoiGianKetThuc { get; set; }
        public string NgayApDung { set; get; }
        public string ThangApDunng { get; set; }
        public string ThuApDung { get; set; }
        public string GioApDung { get; set; }
        public string GhiChu { get; set; }
        public int TrangThai { set; get; }
        public List<KhuyenMaiChiTietMap> KhuyenMaiChiTiets { get; set; }
    }
}
