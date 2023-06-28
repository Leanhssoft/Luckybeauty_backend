using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.HoaDon.HoaDonAnh.Dto
{
    public class CreateOrEditHoaDonAnhDto
    {
        public Guid Id { get; set; }
        public Guid IdHoaDon { get; set; }
        public string URLAnh { get; set; }
    }
}
