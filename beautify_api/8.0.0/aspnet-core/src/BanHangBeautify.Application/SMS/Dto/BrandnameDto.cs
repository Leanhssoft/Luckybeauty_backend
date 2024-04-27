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
        public int TenantId { get; set; }
        public string Brandname { get; set; }
        public string SDTCuaHang { get; set; }
        public DateTime? NgayKichHoat { get; set; }
        public byte? TrangThai { get; set; } = 1;
    }

    public class PageBrandnameDto : BrandnameDto
    {
        public string TenancyName { get; set; }
        public string DisplayTenantName { get; set; }
        public double? TongTienNap { get; set; } = 0;
        public double? DaSuDung { get; set; } = 0;
        public double? ConLai { get; set; } = 0;
        public string TxtTrangThai { get; set; }
    }

    public class ParamSearchBrandname : PagedRequestDto
    {
        /// <summary>
        /// 0. chua kichhoat, 1.kichhoat
        /// </summary>
        public List<byte?> TrangThais { get; set; }
    }
}
