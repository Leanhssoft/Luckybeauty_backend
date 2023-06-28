using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using BanHangBeautify.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;

namespace BanHangBeautify.Entities
{
    public class QuyHoaDon_ChiTiet : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        [Required]
        public int TenantId { get; set; } = 1;
        public Guid IdQuyHoaDon { get; set; }
        [ForeignKey("IdQuyHoaDon")]
        public QuyHoaDon QuyHoaDon { get; set; }
        public Guid? IdHoaDonLienQuan { get; set; }
        [ForeignKey("IdHoaDonLienQuan")]
        public BH_HoaDon BH_HoaDon { get; set; }
        public Guid? IdKhachHang { get; set; }
        [ForeignKey("IdKhachHang")]
        public DM_KhachHang DM_KhachHang { get; set; }
        public Guid? IdNhanVien { get; set; }
        [ForeignKey("IdNhanVien")]
        public NS_NhanVien NS_NhanVien { get; set; }
        public Guid? IdTaiKhoanNganHang { get; set; }
        [ForeignKey("IdTaiKhoanNganHang")]
        public DM_TaiKhoanNganHang DM_TaiKhoanNganHang { get; set; }
        public Guid? IdKhoanThuChi { get; set; }
        [ForeignKey("IdKhoanThuChi")]
        public DM_KhoanThuChi DM_KhoanThuChi { get; set; }

        // Dùng khi khách quẹt thẻ, và phải trả thêm 1 khoản phí gọi là Phí cà thẻ
        // ---> khi đó, phải cài đặt thêm phí ở DM_NganHang"				
        public float? LaPTChiPhiNganHang { get; set; } = 0;
        public float? ChiPhiNganHang { get; set; } = 0;
        public float? ThuPhiTienGui { get; set; } = 0;
        // Nếu khách thanh toán = đổi điểm, DiemThanhToan = số điểm quy đổi, TienThu = số tiền quy đổi từ điểm)
        public float? DiemThanhToan { get; set; } = 0;
        // HinhThucThanhToan
        // 1. Tiền mặt	
        // 2. Pos	
        // 3. Chuyển khoản	
        // 4. Thẻ giá trị(khách nạp sẵn tiền vào Thẻ, sau đó dùng thẻ này để thanh toán)
        // 5. Sử dụng điểm
        public byte HinhThucThanhToan { get; set; } = 1; // byte (C#) = tinyint (sql)
        public float? TienThu { get; set; } = 0;
    }
}
