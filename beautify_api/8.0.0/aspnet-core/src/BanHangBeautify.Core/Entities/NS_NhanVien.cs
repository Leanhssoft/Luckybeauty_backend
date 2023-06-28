using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using BanHangBeautify.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanHangBeautify.Data.Entities
{
    [Table("NS_NhanVien")]
    public class NS_NhanVien : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        [MaxLength(50)]
        public string MaNhanVien { get; set; }
        [MaxLength(20)]
        public string Ho { set; get; }
        [MaxLength(50)]
        public string TenLot { set; get; }
        [MaxLength(256)]
        public string TenNhanVien { get; set; }
        [MaxLength(2000)]
        public string DiaChi { get; set; }
        [MaxLength(256)]
        public string SoDienThoai { get; set; }
        [MaxLength(256)]
        public string CCCD { get; set; }
        public DateTime? NgaySinh { get; set; }
        public byte? KieuNgaySinh { get; set; } = 0;
        public byte? GioiTinh { get; set; }
        [MaxLength(256)]
        public string NgayCap { get; set; }
        [MaxLength(2000)]
        public string NoiCap { get; set; }
        public string Avatar { get; set; }

        //public Guid? IdPhongBan { get; set; }
        //[ForeignKey("IdPhongBan")]
        //public DM_PhongBan DM_PhongBan { get; set; }
        public Guid? IdChucVu { set; get; }
        [ForeignKey("IdChucVu")]
        public NS_ChucVu NS_ChucVu { get; set; }

        public int TenantId { get; set; }

        public static implicit operator string(NS_NhanVien v)
        {
            throw new NotImplementedException();
        }
    }
}
