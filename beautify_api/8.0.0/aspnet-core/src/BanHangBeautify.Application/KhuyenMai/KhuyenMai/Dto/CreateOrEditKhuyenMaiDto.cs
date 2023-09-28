using BanHangBeautify.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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
        public DateTime? ThoiGianKetThuc { get; set; }
        public bool TatCaKhachHang { get; set; }
        public bool TatCaChiNhanh { get; set; }
        public bool TatCaNhanVien { get; set; }
        public List<string> NgayApDung { set; get; }
        public List<string> ThangApDung { get; set; }
        public List<string> ThuApDung { get; set; }
        public List<string> GioApDung { get; set; }
        public List<Guid> IdNhanViens { set; get; }
        public List<Guid> IdChiNhanhs { set; get; }
        public List<Guid> IdNhomKhachs { set; get; }
        public List<KhuyenMaiChiTiet> KhuyenMaiChiTiets { set; get; }
        public string GhiChu { get; set; }
        public int TrangThai { get; set; }
    }
    public class KhuyenMaiChiTiet
    {
        public Guid Id { get; set; }
        public float? TongTienHang { get; set; } = 0;
        public float? GiamGia { get; set; } = 0;
        public bool? GiamGiaTheoPhanTram { get; set; } = true;
        public Guid? IdNhomHangMua { get; set; }
        public Guid? IdDonViQuiDoiMua { get; set; }
        public Guid? IdNhomHangTang { get; set; }
        public Guid? IdDonViQuiDoiTang { get; set; }
        public float? SoLuongMua { get; set; } = 0;
        public float? SoLuongTang { get; set; } = 0;
        public float? GiaKhuyenMai { get; set; } = 0;
        public float? SoDiemTang { set; get; } = 0;
    }
}
