using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.KhachHang.KhachHang.Dto
{
    public class CustomerBasicDto
    {
        public Guid Id { get; set; }
        public Guid IdKhachHang { get { return Id; } }
        public string MaKhachHang { get; set; }
        public string TenKhachHang { get; set; }
        public string SoDienThoai { get; set; }
    }
}
