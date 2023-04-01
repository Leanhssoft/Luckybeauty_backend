using Abp.Application.Services.Dto;
using System;

namespace BanHangBeautify.KhachHang.NhomKhach.Dto
{
    public class NhomKhachDto : EntityDto<Guid>
    {
        public string MaNhomKhach { get; set; }
        public string TenNhomKhach { get; set; }
        public string MoTa { get; set; }
        public int TrangThai { get; set; }
    }
}