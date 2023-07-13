using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BanHangBeautify.Data.Entities;

namespace BanHangBeautify.Entities
{
    public class BH_NhanVienThucHien : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        [Required]
        public int TenantId { get; set; } = 1;
        public Guid IdNhanVien { get; set; }
        [ForeignKey("IdNhanVien")]
        public NS_NhanVien NS_NhanVien { get; set; }
        public Guid? IdHoaDon { get; set; }
        [ForeignKey("IdHoaDon")]
        public BH_HoaDon BH_HoaDon { get; set; }
        public Guid? IdHoaDonChiTiet { get; set; }
        [ForeignKey("IdHoaDonChiTiet")]
        public BH_HoaDon_ChiTiet BH_HoaDon_ChiTiet { get; set; }
        public Guid? IdQuyHoaDon { get; set; }// Nếu chiết khấu theo hóa đơn, và theo % thực thu, --> lưu id phiếu thu (FK to QuyHoaDon)
        [ForeignKey("IdQuyHoaDon")]
        public QuyHoaDon QuyHoaDon { get; set; }
        public double? PTChietKhau { get; set; } = 0;
        public double? TienChietKhau { get; set; } = 0;
        public float? HeSo { get; set; } = 1;
        public bool? ChiaDeuChietKhau { get; set; } = false;
        public bool? TinhHoaHongTruocCK { get; set; } = false; // Nếu dịch vụ dc chiết khấu, nhưng NV vẫn dc hưởng 100% đơn giá gốc ban đầu
        public byte? LoaiChietKhau { get; set; } = 1;// Nếu chiết khấu theo Hóa đơn (1.Thực thu, 2.DoanhThu, 3.VND)
                                                     // Nếu chiết khấu theo Dịch vụ (1.NV thực hiện, 2.NV tư vấn, 3.NV thực hiện theo yêu cầu)
    }
}
