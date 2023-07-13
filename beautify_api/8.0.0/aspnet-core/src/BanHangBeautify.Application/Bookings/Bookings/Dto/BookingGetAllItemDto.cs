using NPOI.OpenXmlFormats.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Bookings.Bookings.Dto
{
    public class BookingGetAllItemDto
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Customer { get; set; }
        public Guid SourceId { get; set; }
        public string Employee { get; set; }
        public string Services { get; set; }
        public string Color { get; set; }
        public string DayOfWeek { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
