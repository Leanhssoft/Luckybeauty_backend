using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.NhanSu.LichLamViec.Dto
{
    public class CreateOrEditLichLamViecDto
    {
        public Guid Id { get; set; }
        public Guid IdChiNhanh { get; set; }
        public Guid IdNhanVien { get; set; }
        public Guid IdCaLamViec { get; set; }
        public DateTime TuNgay { get; set; }
        public DateTime? DenNgay { get; set; }
        public bool LapLai { get; set; }
        public int KieuLapLai { get; set; }
        public int GiaTriLap { get; set; }
        public List<string> NgayLamViec { set; get; }
    }
}
