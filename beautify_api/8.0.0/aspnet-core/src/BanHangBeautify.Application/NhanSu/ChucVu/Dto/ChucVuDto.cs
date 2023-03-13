using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.NhanSu.ChucVu.Dto
{
    public class ChucVuDto:EntityDto<Guid>
    {
        public string MaChucVu { get; set; }
        public string TenChucVu { get; set; }
        public string MoTa { get; set; }
        public int TrangThai { get; set; }
    }
}
