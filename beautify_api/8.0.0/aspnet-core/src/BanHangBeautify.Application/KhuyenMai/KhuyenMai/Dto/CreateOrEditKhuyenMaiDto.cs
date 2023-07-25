using System;

namespace BanHangBeautify.KhuyenMai.KhuyenMai.Dto
{
    public class CreateOrEditKhuyenMaiDto
    {
        public Guid Id { get; set; }
        public string MaKhuyenMai { get; set; }
        public string TenKhuyenMai { get; set; }
        public byte LoaiKhuyenMai { get; set; }
        public byte HinhThucKM { get; set; }
        public DateTime ThoiGianApDung { get; set; }
        public DateTime ThoiGianKetThuc { get; set; }
        public bool TatCaKhachHang { get; set; }
        public bool TatCaChiNhanh { get; set; }
        public bool TatCaNhanVien { get; set; }
        public string NgayApDung { set; get; }
        public string ThangApDunng { get; set; }
        public string ThuApDung { get; set; }
        public string GioApDung { get; set; }
        public string GhiChu { get; set; }
    }
}
