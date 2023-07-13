using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.HoaDon.NhanVienThucHien.Dto
{
    public class CreateOrEditNhanVienThucHienDto
    {
        public Guid Id { set; get; }
        public Guid IdNhanVien { set; get; }
        public Guid IdHoaDon { set; get; }
        public Guid IdChiTietHoaDon { set; get; }
        public Guid IdQuyHoaDon { set; get; }
        public double? PTChietKhau { set; get; }
        public double? TienChietKhau { set; get; }
        public bool ChiaDeuChietKhau { set; get; }
        public double? HeSo { set; get; }
        public bool TinhHoaHongTruocCK { set; get; }
        public byte LoaiChietKhau { set; get; }
    }
}
