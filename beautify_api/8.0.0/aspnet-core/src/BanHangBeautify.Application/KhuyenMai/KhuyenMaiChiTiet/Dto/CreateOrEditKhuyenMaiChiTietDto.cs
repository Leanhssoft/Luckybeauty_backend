using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.KhuyenMai.KhuyenMaiChiTiet.Dto
{
    public class CreateOrEditKhuyenMaiChiTietDto
    {
        public Guid Id { set; get; }
        public Guid IdKhuyenMai { set; get; }
        public byte STT { set; get; }
        public float TongTienHang { set; get; } = 0;
        public float? GiamGia { set; get; } = 0;
        public bool GiamGiaTheoPhanTram { set; get; }
        public Guid IdNhomHangMua { set; get; }
        public Guid IdDonViQuyDoiMua { set; get; }
        public Guid IdNhomHangTang { set; get; }
        public Guid IdDonViQuyDoiTang { set; get; }
        public int SoLuongMua { set; get; }
        public int SoLuongTang { set; get; }
        public float? GiaKhuyenMai { set; get; } = 0;
    }
}
