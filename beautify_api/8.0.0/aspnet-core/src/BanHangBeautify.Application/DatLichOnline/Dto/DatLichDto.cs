using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.DatLichOnline.Dto
{
    public class DatLichDto
    {
        public string TenKhachHang { get; set; }
        public string SoDienThoai { get; set; }
        public Guid IdChiNhanh { get; set; }
        public Guid? IdNhanVien { get; set; }
        public Guid IdDichVu { get; set; }
        public DateTime BookingDate { get; set; }
        public string StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string GhiChu { get; set; }
        public double SoPhutThucHien { get; set; }
    }
}
