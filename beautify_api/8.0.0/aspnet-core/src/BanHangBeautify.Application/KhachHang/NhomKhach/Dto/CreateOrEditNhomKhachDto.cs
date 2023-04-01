using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;

namespace BanHangBeautify.KhachHang.NhomKhach.Dto
{
    public class CreateOrEditNhomKhachDto : EntityDto<Guid>
    {
        [MaxLength(50)]
        public string MaNhomKhach { get; set; }
        [MaxLength(256)]
        public string TenNhomKhach { get; set; }
        [MaxLength(2000)]
        public string MoTa { get; set; }
        public int TrangThai { get; set; }
    }
}