using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.NhatKyHoatDong.Dto
{
    public class NhatKyThaoTacItemDto
    {
        public string ChucNang { get; set; }
        public int LoaNhatKy { get; set; }
        public string NoiDung { get; set; }
        public string NoiDungChiTiet { get; set; }
        public DateTime CreateTime { get; set; }
        public string TenNguoiThaoTac { get; set; }
    }
}
