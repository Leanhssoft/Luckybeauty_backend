using System;

namespace BanHangBeautify.NhanSu.LichLamViec.Dto
{
    public class LichLamViecDto
    {
        public Guid Id { get; set; }
        public DateTime TuNgay { get; set; }
        public DateTime? DenNgay { get; set; }
        public bool LapLai { get; set; }
        public int KieuLapLai { get; set; }
        public int GiaTriLap { get; set; }
    }
}
