using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.DataExporting.Checkin.Dto
{
    public class CheckInHoaDonDto
    {
        public Guid IdCheckIn { get; set; }
        public Guid? IdHoaDon { get; set; }
        public Guid? IdBooking { get; set; }
        public int? TrangThai { get; set; } = 1;
    }
}
