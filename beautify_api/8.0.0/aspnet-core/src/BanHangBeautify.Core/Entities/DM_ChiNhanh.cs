using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanHangBeautify.Entities
{
    public class DM_ChiNhanh : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public Guid IdCongTy { set; get; }
        [ForeignKey("IdCongTy")]
        public HT_CongTy HT_CongTy { get; set; }
        public int TenantId { get; set; }
        [MaxLength(50)]
        public string MaChiNhanh { get; set; }
        [MaxLength(2000)]
        public string TenChiNhanh { get; set; }
        [MaxLength(256)]
        public string SoDienThoai { get; set; }
        [MaxLength(2000)]
        public string DiaChi { get; set; }
        [MaxLength(256)]
        public string MaSoThue { get; set; }
        [MaxLength(2000)]
        public string Logo { get; set; }
        [MaxLength(2000)]
        public string GhiChu { get; set; }
        public int TrangThai { get; set; }
        public DateTime? NgayHetHan { get; set; }
        public DateTime? NgayApDung { get; set; }
        public Guid? NguoiTao { get; set; }
        public Guid? NguoiSua { get; set; }
    }
}
