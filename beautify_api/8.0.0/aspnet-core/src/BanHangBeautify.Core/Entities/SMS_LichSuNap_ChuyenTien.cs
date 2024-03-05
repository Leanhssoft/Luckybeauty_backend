using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using BanHangBeautify.Authorization.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanHangBeautify.Entities
{
    public class SMS_LichSuNap_ChuyenTien : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public Guid? IdPhieuNapTien { get; set; } // Lấy từ bảng QuyHoaDon của HOST
        public DateTime ThoiGianNap_ChuyenTien { get; set; }
        public long? IdNguoiChuyenTien { get; set; } // Nếu nạp tiền: null, Nếu chuyển tiền: lưu IdNhanVien chuyển tiền
        [ForeignKey(nameof(IdNguoiChuyenTien))]
        public User NguoiChuyenTien { get; set; }
        public long? IdNguoiNhanTien { get; set; }// Nếu nạp tiền: lưu IdTaiKhoan Admin, Nếu nhận tiền: lưu IdNhanVien được chuyển tiền
        [ForeignKey(nameof(IdNguoiNhanTien))]
        public User NguoiNhanTien { get; set; }
        public double? SoTienChuyen_Nhan { get; set; } = 0;
        public string NoiDungChuyen_Nhan { get; set; }
    }
}
