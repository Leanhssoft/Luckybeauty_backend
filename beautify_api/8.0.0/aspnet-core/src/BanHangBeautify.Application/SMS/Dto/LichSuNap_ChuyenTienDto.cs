using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.SMS.Dto
{
    public class LichSuNap_ChuyenTienDto
    {
        public Guid? IdPhieuNapTien { get; set; } // Lấy từ bảng QuyHoaDon của HOST
        public DateTime ThoiGianNap_ChuyenTien { get; set; }
        public long? IdNguoiChuyenTien { get; set; } // Nếu nạp tiền: null, Nếu chuyển tiền: lưu IdNhanVien chuyển tiền
        public long? IdNguoiNhanTien { get; set; }// Nếu nạp tiền: lưu IdTaiKhoan Admin, Nếu nhận tiền: lưu IdNhanVien được chuyển tiền
        public double? SoTienChuyen_Nhan { get; set; } = 0;
        public string NoiDungChuyen_Nhan { get; set; }
    }
}
