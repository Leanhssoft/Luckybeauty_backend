using Abp.Authorization.Users;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using BanHangBeautify.Authorization.Users;
using BanHangBeautify.Data.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanHangBeautify.Entities
{
    public class HeThong_SMS : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public long? IdNguoiGui { set; get; }// cửa hàng tự quản lý số tiền tối đa cho phép NV được gửi
        [ForeignKey(nameof(IdNguoiGui))]
        public User User { get; set; }
        public Guid IdChiNhanh { set; get; }
        [ForeignKey(nameof(IdChiNhanh))]
        public DM_ChiNhanh DM_ChiNhanh { get; set; }
        public Guid? IdKhachHang { set; get; }
        [ForeignKey(nameof(IdKhachHang))]
        public DM_KhachHang DM_KhachHang { get; set; }
        public string IdTinNhan { set; get; }// dc trả về = ESMS API (vd: a4e3472e-5ff8-4a03-beb6-38b3393a42b5186 (gần giống Guid nhưng không phải - vì chuỗi cuối cùng gồm 15 kí tự))
        public string SoDienThoai { set; get; }
        public int SoTinGui { set; get; }
        [MaxLength(4000)]
        public string NoiDungTin { set; get; }
        public DateTime ThoiGianGui { set; get; }
        public byte IdLoaiTin { set; get; }// enum DS loaitin (1. Tin giao dịch, 2. Tin sinh nhật, 3. Tin thường, 4. Tin lịch hẹn, 32. nhắc lịch hẹn, 32. xác nhận lịch hẹn)
        public double? GiaTienMoiTinNhan { get; set; } = 950;// hiện tại, đang mặc định giá này cho all nhà mạng (nếu sau cần thì thêm data)
        public int? TrangThai { set; get; }// gửi thành công, thất bại,...
        public byte? HinhThucGui { set; get; }// 1.sms, 2.zalo, 3.gmail
    }
}
