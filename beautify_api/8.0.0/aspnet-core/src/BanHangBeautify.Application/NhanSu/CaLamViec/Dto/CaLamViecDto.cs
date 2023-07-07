using System;

namespace BanHangBeautify.NhanSu.CaLamViec.Dto
{
    public class CaLamViecDto
    {
        public Guid Id { get; set; }
        public string MaCa { get; set; }
        public string TenCa { get; set; }
        public string GioVao { get; set; }
        public string GioRa { get; set; }
        public float TongGioCong { get; set; }
    }
}
