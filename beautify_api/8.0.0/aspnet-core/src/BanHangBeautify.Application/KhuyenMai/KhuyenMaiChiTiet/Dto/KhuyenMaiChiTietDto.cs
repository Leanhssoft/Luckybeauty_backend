using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.KhuyenMai.KhuyenMaiChiTiet.Dto
{
    public class KhuyenMaiChiTietDto
    {
        public Guid Id{set;get;}
        public Guid IdKhuyenMai{set;get;}
        public byte STT{set;get;}
        public decimal TongTienHang{set;get;}
        public decimal GiamGia{set;get;}
        public bool GiamGiaTheoPhanTram{set;get;}
        public Guid IdNhomHangMua{set;get;}
        public Guid IdDonViQuyDoiMua{set;get;}
        public Guid IdNhomHangTang{set;get;}
        public Guid IdDonViQuyDoiTang{set;get;}
        public int SoLuongMua{set;get;}
        public int SoLuongTang{set;get;}
        public decimal GiaKhuyenMai{set;get;}
    }
}
