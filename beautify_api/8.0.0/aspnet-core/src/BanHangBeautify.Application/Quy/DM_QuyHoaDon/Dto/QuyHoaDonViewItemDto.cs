using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Quy.DM_QuyHoaDon.Dto
{
    public class QuyHoaDonViewItemDto
    {
        public Guid Id { get; set; }
        public Guid? IdChiNhanh { get; set; }
        public string LoaiPhieu { get; set; }
        public string MaPhieu { get; set; }
        public string ThoiGianTao { get; set; }
        public string LoaiThuChi { get; set; }
        public decimal TongTienThu { get; set; }
        public string HinhThucThanhToan { get; set; }
        public string MaNguoiNhan { get; set; }
        public string TenNguoiNhan { get; set; }
        public string TrangThai { get; set; }
    }
}
