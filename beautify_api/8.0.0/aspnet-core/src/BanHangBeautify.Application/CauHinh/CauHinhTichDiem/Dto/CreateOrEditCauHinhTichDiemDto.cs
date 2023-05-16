using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.CauHinh.CauHinhTichDiem.Dto
{
    public class CreateOrEditCauHinhTichDiemDto
    {
        public Guid Id { set; get; }
        public Guid IdCauHinh { set; get; }
        public decimal TyLeDoiDiem { set; get; }
        public bool ChoPhepThanhToanBangDiem { set; get; }
        public decimal DiemThanhToan { set; get; }
        public decimal TienThanhToan { set; get; }
        public bool KhongTichDiemHDGiamGia { set; get; }
        public bool TichDiemHoaDonGiamGia { set; get; }
        public bool KhongTichDiemSPGiamGia { set; get; }
        public bool TatCaKhachHang { set; get; }
        public int SoLanMua { set; get; }
    }
}
