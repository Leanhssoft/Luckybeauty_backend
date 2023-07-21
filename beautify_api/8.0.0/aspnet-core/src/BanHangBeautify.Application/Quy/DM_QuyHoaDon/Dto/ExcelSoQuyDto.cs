using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Quy.DM_QuyHoaDon.Dto
{
    public class ExcelSoQuyDto
    {
        public string LoaiPhieu { get; set; }// 11.thu/12.chi
        public string MaHoaDon { get; set; }
        public DateTime? NgayLapHoaDon { get; set; }
        public string TenNguoiNop { get; set; }
        public double? TongTienThu { get; set; }
        public string SHinhThucThanhToan { get; set; }// mat, pos, ck
        public string TxtTrangThai { get; set; }
    }
}
