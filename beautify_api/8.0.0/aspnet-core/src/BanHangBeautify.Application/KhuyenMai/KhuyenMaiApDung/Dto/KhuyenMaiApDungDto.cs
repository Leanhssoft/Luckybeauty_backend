using System;

namespace BanHangBeautify.KhuyenMai.KhuyenMaiApDung.Dto
{
    public class KhuyenMaiApDungDto
    {
        public Guid Id { get; set; }
        public Guid IdKhuyenMai { set; get; }
        public Guid? IdChiNhanh { set; get; }
        public Guid? IdNhomKhach { set; get; }
        public Guid? IdNhanVien { set; get; }
    }
}
