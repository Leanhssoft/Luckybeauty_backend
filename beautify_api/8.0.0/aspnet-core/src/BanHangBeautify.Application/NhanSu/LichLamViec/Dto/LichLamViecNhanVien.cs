using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.NhanSu.LichLamViec.Dto
{
    public class LichLamViecNhanVien
    {
        public Guid Id { get; set; }
        public Guid IdNhanVien { get; set; }
        public string Avatar { get; set; }
        public string TenNhanVien { get; set; }
        public string TongThoiGian { get; set; }
        public string Monday { get; set; }
        public string Tuesday { get; set; }
        public string Wednesday { get; set; }
        public string Thursday { get; set; }
        public string Friday { get; set; }
        public string Saturday { get; set; }
        public string SunDay { get; set; }
    }
}
