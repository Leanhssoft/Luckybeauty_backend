using System;

namespace BanHangBeautify.CauHinh.CauHinhTichDiem.Dto
{
    public class CreateOrEditCauHinhTichDiemDto
    {
        public Guid Id { set; get; }
        public Guid IdCauHinh { set; get; }
        public decimal TyLeDoiDiem { set; get; }
        public bool ChoPhepThanhToanBangDiem { set; get; }
        public float? DiemThanhToan { set; get; } = 0;
        public float TienThanhToan { set; get; } = 0;
        public bool KhongTichDiemHDGiamGia { set; get; }
        public bool TichDiemHoaDonGiamGia { set; get; }
        public bool KhongTichDiemSPGiamGia { set; get; }
        public bool TatCaKhachHang { set; get; }
        public int SoLanMua { set; get; }
    }
}
