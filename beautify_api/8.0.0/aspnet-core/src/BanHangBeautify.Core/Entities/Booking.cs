using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanHangBeautify.Entities
{
    public class Booking : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public Guid? IdKhachHang { set; get; }
        [ForeignKey("IdKhachHang")]
        public DM_KhachHang DM_KhachHang { get; set; }
        public Guid? IdChiNhanh { get; set; }
        [ForeignKey(nameof(IdChiNhanh))]
        public DM_ChiNhanh DM_ChiNhanh { set; get; }
        [MaxLength(100)]
        public string BookingCode { get; set; }
        public string TenKhachHang { get; set; }
        public string SoDienThoai { set; get; }
        public DateTime StartTime { set; get; }
        public DateTime? EndTime { set; get; }
        public DateTime BookingDate { get; set; }
        public byte LoaiBooking { get; set; }
        public int TrangThai { get; set; } // Huy = 0,   DatLich = 1,  DaXacNhan = 2,   CheckIn = 3,   HoanThanh = 4
        public string GhiChu { get; set; }
        public Guid? UserXuLy { get; set; }
        public DateTime? NgayXuLy { get; set; }
    }
}
