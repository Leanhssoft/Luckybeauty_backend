using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanHangBeautify.Entities
{
    public class BH_HoaDon_ChiTiet : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        [Required]
        public int TenantId { get; set; } = 1;
        public Guid IdHoaDon { get; set; }
        [ForeignKey("IdHoaDon")]
        public BH_HoaDon BH_HoaDon { get; set; }
        public int STT { get; set; } = 1;
        public Guid IdDonViQuyDoi { get; set; }
        [ForeignKey("IdDonViQuyDoi")]
        public DM_DonViQuiDoi DM_DonViQuiDoi { get; set; }
        public Guid? IdChiTietHoaDon { get; set; }// sử dụng khi trả hàng, lưu chi tiết hóa đơn gốc ban đầu
        [ForeignKey("IdChiTietHoaDon")]
        public BH_HoaDon_ChiTiet ChiTietHoaDonParent { get; set; }
        public double? SoLuong { get; set; } = 0;
        public double? DonGiaTruocCK { get; set; } = 0;
        public double? ThanhTienTruocCK { get; set; } = 0;
        public double? PTChietKhau { get; set; } = 0;
        public double? TienChietKhau { get; set; } = 0;
        public double? DonGiaSauCK { get; set; } = 0;
        public double? ThanhTienSauCK { get; set; } = 0;
        public double? PTThue { get; set; } = 0;
        public double? TienThue { get; set; } = 0;
        public double? DonGiaSauVAT { get; set; } = 0;
        public double? ThanhTienSauVAT { get; set; } = 0;
        public double? TonLuyKe { get; set; } = 0;
        [MaxLength(4000)] public string GhiChu { get; set; } = string.Empty;
        public int TrangThai { get; set; } = 1;// 0.Xóa, 1.Chưa xóa
    }
}
