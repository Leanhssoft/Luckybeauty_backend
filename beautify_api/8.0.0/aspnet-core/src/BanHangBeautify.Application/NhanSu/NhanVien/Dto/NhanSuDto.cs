﻿using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;

namespace BanHangBeautify.NhanSu.NhanVien.Dto
{
    public class NhanSuDto : EntityDto<Guid>
    {
        public string MaNhanVien { get; set; }
        public string Ho { get; set; }
        public string TenLot { get; set; }
        public string TenNhanVien { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }
        public string CCCD { get; set; }
        public DateTime? NgaySinh { get; set; }
        public byte? KieuNgaySinh { get; set; } = 0;
        public byte? GioiTinh { get; set; } = 1;
        [MaxLength(256)]
        public string NgayCap { get; set; }
        [MaxLength(2000)]
        public string NoiCap { get; set; }
        public byte[] Avatar { get; set; }
        public Guid? IdChucVu { set; get; }
        //public Guid IdPhongBan { get; set; }
    }
}
