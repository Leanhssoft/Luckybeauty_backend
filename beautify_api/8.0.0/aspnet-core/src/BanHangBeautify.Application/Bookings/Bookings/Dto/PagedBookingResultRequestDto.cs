using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Bookings.Bookings.Dto
{
    public class PagedBookingResultRequestDto
    {
        public Guid IdChiNhanh { get; set; }
        public DateTime DateSelected { get; set; }
        public string TypeView { get; set; }
        public Guid? IdNhanVien { get; set; }
        public Guid? IdDichVu { get; set; }
    }
}
