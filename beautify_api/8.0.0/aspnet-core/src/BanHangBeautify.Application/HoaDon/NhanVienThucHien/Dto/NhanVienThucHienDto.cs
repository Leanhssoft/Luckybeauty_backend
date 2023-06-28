using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.HoaDon.NhanVienThucHien.Dto
{
    public class NhanVienThucHienDto
    {
        public Guid? IdNhanVien { set; get; }
        public Guid? IdHoaDon { set; get; }
        public Guid? IdChiTietHoaDon { set; get; }
        public Guid? IdQuyHoaDon { set; get; }
        public float? PTChietKhau { set; get; }
        public float? TienChietKhau { set; get; }
        public bool? ChiaDeuChietKhau { set; get; } = false;
        public float? HeSo { set; get; } = 1;
        public bool? TinhHoaHongTruocCK { set; get; } = false;
        public byte? LoaiChietKhau { set; get; } = 1;
    }
}
