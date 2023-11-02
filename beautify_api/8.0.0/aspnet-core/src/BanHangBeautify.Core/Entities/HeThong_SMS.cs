using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using BanHangBeautify.Data.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanHangBeautify.Entities
{
    public class HeThong_SMS : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public Guid? IdNguoiGui { set; get; }// cửa hàng tự quản lý số tiền tối đa cho phép NV được gửi
        [ForeignKey(nameof(IdNguoiGui))]
        public NS_NhanVien NS_NhanVien { get; set; }
        public Guid IdChiNhanh { set; get; }
        [ForeignKey(nameof(IdChiNhanh))]
        public DM_ChiNhanh DM_ChiNhanh { get; set; }
        public Guid? IdKhachHang { set; get; }
        [ForeignKey(nameof(IdKhachHang))]
        public DM_KhachHang DM_KhachHang { get; set; }
        public Guid? IdHoaDon { set; get; }
        [ForeignKey(nameof(IdHoaDon))]
        public BH_HoaDon BH_HoaDon { get; set; }
        public Guid? IdTinNhan { set; get; }// dc trả về = qua ESMS
        public string SoDienThoai { set; get; }
        public int SoTinGui { set; get; }
        [MaxLength(4000)]
        public string NoiDungTin { set; get; }
        public DateTime ThoiGianGui { set; get; }
        public byte IdLoaiTin { set; get; }// enum DS loaitin (1. Tin giao dịch, 2. Tin sinh nhật, 3. Tin thường, 4. Tin lịch hẹn)
        public double? GiaTienMoiTinNhan { get; set; } = 0;
        public int? TrangThai { set; get; }// gửi thành công, thất bại,...
    }
}
