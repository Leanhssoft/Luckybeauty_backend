using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using BanHangBeautify.Data.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanHangBeautify.Entities
{
    public class NS_QuaTrinh_CongTac : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public Guid IdNhanVien { set; get; }
        [ForeignKey("IdNhanVien")]
        public NS_NhanVien NS_NhanVien { get; set; }
        public Guid? IdPhongBan { set; get; }
        public Guid? IdChiNhanh { set; get; }
        public DateTime? TuNgay { set; get; }
        public DateTime? DenNgay { set; get; }
        public int TrangThai { set; get; }
        public Guid? NguoiTao { set; get; }
        public Guid? NguoiSua { set; get; }
    }
}
