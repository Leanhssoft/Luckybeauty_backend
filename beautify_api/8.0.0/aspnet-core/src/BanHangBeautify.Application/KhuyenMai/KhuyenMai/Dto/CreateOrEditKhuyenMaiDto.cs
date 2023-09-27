﻿using System;
using System.Collections.Generic;

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
        public List<string> NgayApDung { set; get; }
        public List<string> ThangApDung { get; set; }
        public List<string> ThuApDung { get; set; }
        public List<string> GioApDung { get; set; }
        public List<Guid> IdNhaViens { set; get; }
        public List<Guid> IdChiNhanhs { set; get; }
        public List<Guid> IdNhomKhachs { set; get; }
        public decimal TongTienHang { set; get; }
        public bool GiamGiaTheoPhanTram { set; get; }
        public string GhiChu { get; set; }
    }
}
