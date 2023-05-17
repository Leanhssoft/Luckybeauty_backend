using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Quy.QuyHoaDonChiTiet.Dto
{
    public class CreateOrEditQuyHoaDonCTDto
    {
        public Guid Id{set;get;}
        public Guid IdQuyHoaDon{set;get;}
        public Guid? IdHoaDonLienQuan{set;get;}
        public Guid? IdKhachHang{set;get;}
        public Guid? IdNhanVien{set;get;}
        public Guid? IdTaiKhoanNganHang{set;get;}
        public Guid? IdKhoanThuChi{set;get;}
        public float? LaPTChiPhiNganHang { get; set; } = 0;
        public float? ChiPhiNganHang { get; set; } = 0;
        public float? ThuPhiTienGui { get; set; } = 0;
        public float? DiemThanhToan { get; set; } = 0;
        public byte HinhThucThanhToan { get; set; } = 1;
        public float? TienThu { get; set; } = 0;
    }
}
