using BanHangBeautify.KhachHang.KhachHang.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Checkin.Dto
{
    public class PageKhachHangCheckingDto : KhachHangView
    {
        public Guid? IdCheckIn { get; set; }
        public Guid? IdChiNhanh { get; set; }
        public Guid IdKhachHang { get; set; }
        public Guid? IdBooking { get; set; }
        public string DateCheckIn { get; set; }
        public string TimeCheckIn { get; set; }
        public string TxtTrangThaiCheckIn { get; set; }
    }
}
