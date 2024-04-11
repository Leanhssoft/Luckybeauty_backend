using System;

namespace BanHangBeautify.NhatKyHoatDong.Dto
{
    public class NhatKyThaoTacItemDto
    {
        public string ChucNang { get; set; }
        public int LoaiNhatKy { get; set; }
        public string NoiDung { get; set; }
        public string NoiDungChiTiet { get; set; }
        public DateTime CreationTime { get; set; }
        public string TenNguoiThaoTac { get; set; }
        public string ChiNhanh { get; set; }
    }
}
