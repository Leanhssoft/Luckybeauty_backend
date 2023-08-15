using BanHangBeautify.HangHoa.DonViQuiDoi.Dto;
using System;
using System.Collections.Generic;

namespace BanHangBeautify.HangHoa.HangHoa.Dto
{
    public class HangHoaDto
    {
        public Guid Id { get; set; }
        public Guid? IdHangHoa { get; set; }
        public Guid IdDonViQuyDoi { get; set; }
        public Guid? IdNhomHangHoa { get; set; }
        public string MaHangHoa { get; set; }
        public string TenHangHoa { get; set; }
        public string TenNhomHang { get; set; }
        public string Color { get; set; }
        public float? SoPhutThucHien { get; set; }
        public int TrangThai { get; set; }
        public int IdLoaiHangHoa { get; set; }
        public double? GiaBan { get; set; }
        public string MoTa { get; set; }
        public string Image { get; set; }
        public string TenLoaiHangHoa { get; set; }
        public string TxtTrangThaiHang { get; set; }
        public List<DonViQuiDoiDto> DonViTinhs { get; set; }
    }
}
