using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.SMS.Dto
{
    public class BrandnameDto
    {
        public Guid Id { get; set; }
        public string Brandname { get; set; }
        public string SDTCuaHang { get; set; }
        public DateTime? NgayKichHoat { get; set; }
        public byte? TrangThai { get; set; } = 1;
    }

    public class PageBrandnameDto : BrandnameDto
    {
        public double? TongTienNap { get; set; } = 0;
        public double? DaSuDung { get; set; } = 0;
        public double? ConLai { get; set; } = 0;
        public string TxtTrangThai { get; set; }
    }
}
