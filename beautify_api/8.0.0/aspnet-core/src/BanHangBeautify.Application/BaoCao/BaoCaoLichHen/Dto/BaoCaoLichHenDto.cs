﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.BaoCao.BaoCaoLichHen.Dto
{
    public class BaoCaoLichHenDto
    {
        public DateTime BookingDate { get; set; }
        public string TenKhachHang { get; set; }
        public string SoDienThoai { get; set; }
        public string TenHangHoa { get; set; }
        public string GhiChu { get; set; }
        public int TrangThai { get; set; }
    }
}