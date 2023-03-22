using Abp.Application.Services.Dto;
using System;

namespace BanHangBeautify.KhachHang.NhomKhach.Dto
{
    public class NhomKhachDto : EntityDto<Guid>
    {
        public string MaNhom { get; set; }
        public string Tennhom { get; set; }
        public string MoTa { get; set; }
        public int TrangThai { get; set; }
    }
}