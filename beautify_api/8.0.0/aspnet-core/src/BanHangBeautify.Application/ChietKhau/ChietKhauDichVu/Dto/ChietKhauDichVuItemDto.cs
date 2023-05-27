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
        public float GiaTri { get; set; }
        public float? HoaHongThucHien { get; set; }
        public float? HoaHongYeuCauThucHien { get; set; }
        public float? HoaHongTuVan { get; set; }
        public float GiaDichVu { get; set; }
    }
}
