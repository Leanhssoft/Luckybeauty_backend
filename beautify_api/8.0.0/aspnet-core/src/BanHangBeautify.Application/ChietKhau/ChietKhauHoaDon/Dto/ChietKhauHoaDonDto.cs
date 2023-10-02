using System;
using System.Collections.Generic;

namespace BanHangBeautify.ChietKhau.ChietKhauHoaDon.Dto
{
    public class ChietKhauHoaDonDto
    {
        public Guid Id { set; get; }
        public Guid IdChiNhanh { set; get; }
        public byte LoaiChietKhau { set; get; }
        public double GiaTriChietKhau { set; get; }
        public string ChungTuApDung { set; get; }
        public string GhiChu { get; set; }
    }
}
