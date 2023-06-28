using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;

namespace BanHangBeautify.AppDanhMuc.AppChiNhanh.Dto
{
    public class CreateChiNhanhDto : EntityDto<Guid>
    {
        public Guid IdCongTy { set; get; }
        [MaxLength(50)]
        public string MaChiNhanh { get; set; }
        [MaxLength(2000)]
        [Required]
        public string TenChiNhanh { get; set; }
        [MaxLength(256)]
        [Required]
        public string SoDienThoai { get; set; }
        [MaxLength(2000)]
        public string DiaChi { get; set; }
        [MaxLength(256)]
        public string MaSoThue { get; set; }
        [MaxLength(2000)]
        public string Logo { get; set; }
        [MaxLength(2000)]
        public string GhiChu { get; set; }
        public int TrangThai { get; set; }
        [Required]
        public DateTime NgayHetHan { get; set; }
        public DateTime NgayApDung { get; set; }


    }
}
