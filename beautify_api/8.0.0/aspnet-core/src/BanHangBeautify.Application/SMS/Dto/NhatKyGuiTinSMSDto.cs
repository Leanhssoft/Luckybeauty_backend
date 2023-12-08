using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.SMS.Dto
{
    public class NhatKyGuiTinSMSDto
    {
        public Guid IdHeThongSMS { get; set; }
        public Guid IdKhachHang { get; set; }
        public Guid IdChiNhanh { get; set; }
        public Guid? IdHoaDon { get; set; }
        public Guid? IdBooking { get; set; }
        public byte? IdLoaiTin { get; set; } = 1;
        public DateTime? ThoiGianTu { get; set; }
        public DateTime? ThoiGianDen { get; set; }
    }
}
