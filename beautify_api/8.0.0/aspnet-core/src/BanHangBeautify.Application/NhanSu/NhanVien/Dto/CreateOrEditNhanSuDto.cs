using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;

namespace BanHangBeautify.NhanSu.NhanVien.Dto
{
    public class CreateOrEditNhanSuDto : EntityDto<Guid>
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

    }
}
