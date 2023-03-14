﻿using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using BanHangBeautify.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanHangBeautify.Data.Entities
{
    [Table("NS_NhanViens")]
    public class NS_NhanVien : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        [MaxLength(50)]
        public string MaNhanVien { get; set; }
        [MaxLength(256)]
        public string TenNhanVien { get; set; }
        [MaxLength(2000)]
        public string DiaChi { get; set; }
        [MaxLength(256)]
        public string SoDienThoai { get; set; }
        [MaxLength(256)]
        public string CCCD { get; set; }
        public DateTime NgaySinh { get; set; }
        public int KieuNgaySinh { get; set; }
        public int GioiTinh { get; set; }
        [MaxLength(256)]
        public string NgayCap { get; set; }
        [MaxLength(2000)]
        public string NoiCap { get; set; }
        public byte[] Avatar { get; set; }

        //public Guid PhongBan_Id { get; set; }
        public Guid IdChucVu { set; get; }
        [ForeignKey("IdChucVu")]
        public NS_ChucVu NS_ChucVu { get; set; }

        public int TenantId { get; set; }
        public Guid? NguoiTao { get; set; }
        public Guid? NguoiSua { get; set; }
        public DateTime NgayTao { get; set; }
        public DateTime? NgaySua { get; set; }
        public Guid? NguoiXoa { get; set; }
        public DateTime? NgayXoa { get; set; }
    }
}
