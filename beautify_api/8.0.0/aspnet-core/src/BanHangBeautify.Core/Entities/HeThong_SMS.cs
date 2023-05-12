using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using BanHangBeautify.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Entities
{
    public class HeThong_SMS : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public Guid IdChiNhanh{set;get;}
        [ForeignKey(nameof(IdChiNhanh))]
        public DM_ChiNhanh DM_ChiNhanh { get; set; }
        public Guid IdNguoiGui{set;get;}
        [ForeignKey(nameof(IdNguoiGui))]
        public NS_NhanVien NS_NhanVien { get; set; }
        public Guid IdKhachHang{set;get;}
        [ForeignKey(nameof(IdKhachHang))]
        public DM_KhachHang DM_KhachHang { get; set; }
        public Guid? IdHoaDon { set; get; }
        public Guid? IdTinNhan{set;get;}
        public string SoDienThoai{set;get;}
        public int SoTinGui{set;get;}
        [MaxLength(4000)]
        public string NoiDungTin{set;get;}
        public DateTime ThoiGianGui{set;get;}
        public byte LoaiTin{set;get;}
    }
}
