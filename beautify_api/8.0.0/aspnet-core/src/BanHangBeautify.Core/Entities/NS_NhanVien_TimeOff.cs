using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using BanHangBeautify.Data.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanHangBeautify.Entities
{
    public class NS_NhanVien_TimeOff : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { set; get; }
        public Guid IdNhanVien { get; set; }
        [ForeignKey("IdNhanVien")]
        public NS_NhanVien NS_NhanVien { get; set; }
        public DateTime TuNgay { set; get; }
        public DateTime DenNgay { set; get; }
        public int LoaiNghi { get; set; }
        public double TongNgayNghi { set; get; }
    }
}
