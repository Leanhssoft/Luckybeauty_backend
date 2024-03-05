using System;

namespace BanHangBeautify.CauHinh.CauHinhTichDiemChiTiet.Dto
{
    public class CreateOrEditCauHinhTichDiemChiTietDto
    {
        public Guid Id { set; get; }
        public Guid IdTichDiem { set; get; }
        public Guid IdNhomKhachHang { set; get; }
    }
}
