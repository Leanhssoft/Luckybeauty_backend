using BanHangBeautify.KhachHang.KhachHang.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Checkin.Dto
{
    public class PageKhachHangCheckingDto: KhachHangView
    {
        public DateOnly DateCheckIn { get; set; }
        public TimeSpan TimeCheckIn { get; set; }
        public int TxtTrangThaiCheckIn { get; set; }
    }
}
