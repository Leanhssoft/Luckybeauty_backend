using Abp.Application.Services.Dto;
using System;

namespace BanHangBeautify.KhachHang.NguonKhach.Dto
{
    public class NguonKhachDto : EntityDto<Guid>
    {
        public string MaNguon { get; set; }
        public string TenNguon { get; set; }
        public int TrangThai { get; set; }
    }
}