using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanHangBeautify.Entities
{
    public class DatLich : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        //public Guid IdKhachHang { set; get; }
        //[ForeignKey("IdKhachHang")]
        //public DM_KhachHang DM_KhachHang { get; set; }
        public Guid IdChiNhanh { get; set; }
        [ForeignKey(nameof(IdChiNhanh))]
        public DM_ChiNhanh DM_ChiNhanh { set; get; }
        public string TenKhachHang { get; set; }
        public string SoDienThoai { set; get; }
        public DateTime StartTime { set; get; }
        public DateTime EndTime { set; get; }
        public DateTime NgayDatLich { get; set; }
        public int TrangThai { get; set; }
        public string GhiChu { get; set; }
        public DateTime NgayTao { get; set; }
    }
}
