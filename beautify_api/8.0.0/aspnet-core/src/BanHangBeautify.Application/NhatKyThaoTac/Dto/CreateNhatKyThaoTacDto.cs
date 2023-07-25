using System;

namespace BanHangBeautify.NhatKyHoatDong.Dto
{
    public class CreateNhatKyThaoTacDto
    {
        public Guid IdChiNhanh { get; set; }
        public string NoiDung { get; set; }
        public string ChucNang { get; set; }
        public string NoiDungChiTiet { get; set; }
        public int LoaiNhatKy { set; get; }
    }
}
