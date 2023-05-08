using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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
        public float? SoLuong { get; set; } = 0;
        public float? DonGiaTruocCK { get; set; } = 0;
        public float? ThanhTienTruocCK { get; set; } = 0;
        public float? PTChietKhau { get; set; } = 0;
        public float? TienChietKhau { get; set; } = 0;
        public float? DonGiaSauCK { get; set; } = 0;
        public float? ThanhTienSauCK { get; set; } = 0;
        public float? PTThue { get; set; } = 0;   
        public float? TienThue { get; set; } = 0;
        public float? DonGiaSauVAT { get; set; } = 0;
        public float? ThanhTienSauVAT { get; set; } = 0;
        public float? TonLuyKe { get; set; } = 0;
        [MaxLength(4000)] public string GhiChu { get; set; } = string.Empty;
        public int TrangThai { get; set; } = 0;// 0.Xóa, 1.Chưa xóa
    }
}
