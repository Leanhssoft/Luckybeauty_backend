using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Quy.DM_QuyHoaDon.Dto
{
    public class GetAllQuyHoaDonItemDto
    {
        public Guid Id { set; get; }
        public Guid? IdChiNhanh { set; get; }
        public string LoaiPhieu { get; set; }// 11.thu/12.chi
        public string MaPhieu { get; set; }
        public DateTime ThoiGianTao { get; set; }
        public string LoaiThuChi { get; set; }
        public float TongTienThu { get; set; }
        public string HinhThucThanhToan { get; set; }
        public string TrangThai { get; set; }
    }
}
