using System;

namespace BanHangBeautify.NhatKyHoatDong.Dto
{
    public class NhatKyThaoTacDto
    {
        public Guid Id { get; set; }
        public string ChucNang { get; set; }
        public int LoaNhatKy { get; set; }
        public string NoiDung { get; set; }
        public string NoiDungChiTiet { get; set; }
        public DateTime CreateTime { get; set; }
        public int UserId { get; set; }
        public Guid IdNhanVien { get; set; }
    }
}
