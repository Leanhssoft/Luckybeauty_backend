using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;

namespace BanHangBeautify.KhachHang.NguonKhach.Dto
{
    public class CreateOrEditNguonKhachDto : EntityDto<Guid>
    {
        public string MaNguon { get; set; }
        [MaxLength(256)]
        public string TenNguon { get; set; }
        public int TrangThai { get; set; }
    }
}