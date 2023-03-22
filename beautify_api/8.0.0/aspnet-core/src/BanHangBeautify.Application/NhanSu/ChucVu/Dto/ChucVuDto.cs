using Abp.Application.Services.Dto;
using System;

namespace BanHangBeautify.NhanSu.ChucVu.Dto
{
    public class ChucVuDto : EntityDto<Guid>
    {
        public string MaChucVu { get; set; }
        public string TenChucVu { get; set; }
        public string MoTa { get; set; }
        public int TrangThai { get; set; }
    }
}
