using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.ChietKhau.ChietKhauDichVu.Dto
{
    public class ChietKhauDichVuItemDto
    {
        public Guid Id { get; set; }
        public string TenDichVu { get; set; }
        public string TenNhomDichVu { get; set; }
        public double GiaTri { get; set; }
        public bool LaPhanTram { set; get; }
        public double? HoaHongThucHien { get; set; }
        public double? HoaHongYeuCauThucHien { get; set; }
        public double? HoaHongTuVan { get; set; }
        public double GiaDichVu { get; set; }
    }
}
