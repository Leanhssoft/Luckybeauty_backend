using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.NhanSu.ChucVu.Dto
{
    public class CreateOrEditChucVuDto :EntityDto<Guid>
    {
        [Required]
        public string MaChucVu { get; set; }
        [MaxLength(256)]
        [Required]
        public string TenChucVu { get; set; }
        [MaxLength(2000)]
        public string MoTa { get; set; }
        public int TrangThai { get; set; }
    }
}
