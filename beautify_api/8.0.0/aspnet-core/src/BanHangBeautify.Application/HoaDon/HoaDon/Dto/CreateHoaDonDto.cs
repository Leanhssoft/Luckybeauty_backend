using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.HoaDon.HoaDon.Dto
{
    public class CreateHoaDonDto
    {
        public int IdLoaiChungTu { get; set; }
        public Guid IdChiNhanh { get; set; }
        public Guid? IdKhachHang { get; set; }
        public Guid? IdNhanVien { get; set; }
        public Guid? IdViTriPhong { get; set; }
        public Guid? IdHoaDon { get; set; }
        public int TrangThai { get; set; }
        public DateTime NgayLapHoaDon { get; set; }
        public string MaHoaDon { get; set; }
        public DateTime? NgayApDung { get; set; }
        public DateTime? NgayHetHan { get; set; }
        public string GhiChu { get; set; }
    }
}
