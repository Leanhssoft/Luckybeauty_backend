using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.DataExporting.Checkin.Dto
{
    public class KHCheckInDto
    {
        public Guid Id { get; set; }
        public Guid? IdChiNhanh { get; set; }
        public Guid IdKhachHang { get; set; }
        public DateTime DateTimeCheckIn { get; set; }
        public string GhiChu { get; set; } = string.Empty;
        public int TrangThai { get; set; } = 1;
    }
}
