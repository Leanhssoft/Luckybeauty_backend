using System;

namespace BanHangBeautify.ChietKhau.ChietKhauHoaDon.Dto
{
    public class ChietKhauHoaDonItemDto
    {
        public Guid Id { get; set; }
        public string GiaTriChietKhau { get; set; }
        public string ChungTuApDung { get; set; }
        public string GhiChu { get; set; }
        public byte LoaiChietKhau { set; get; }
    }
}
