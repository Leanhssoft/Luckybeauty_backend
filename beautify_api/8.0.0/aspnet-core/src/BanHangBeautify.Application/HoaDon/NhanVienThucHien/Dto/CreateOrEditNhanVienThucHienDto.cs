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
        public float? PTChietKhau { set; get; }
        public float? TienChietKhau { set; get; }
        public bool ChiaDeuChietKhau { set; get; }
        public float? HeSo { set; get; }
        public bool TinhHoaHongTruocCK { set; get; }
        public byte LoaiChietKhau { set; get; }
    }
}
