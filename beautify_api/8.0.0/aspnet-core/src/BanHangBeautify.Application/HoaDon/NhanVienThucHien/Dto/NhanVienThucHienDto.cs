using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.HoaDon.NhanVienThucHien.Dto
{
    public class NhanVienThucHienDto
    {
        public Guid Id { set; get; }
        public Guid IdNhanVien { set; get; }
        public Guid IdHoaDon { set; get; }
        public Guid IdChiTietHoaDon { set; get; }
        public Guid IdQuyHoaDon { set; get; }
        public decimal PTChietKhau{set;get;}

public decimal TienChietKhau{set;get;}
public bool ChiaDeuChietKhau{set;get;}

public decimal HeSo{set;get;}
public bool TinhHoaHongTruocCK{set;get;}

public byte LoaiChietKhau{set;get;}
    }
}
