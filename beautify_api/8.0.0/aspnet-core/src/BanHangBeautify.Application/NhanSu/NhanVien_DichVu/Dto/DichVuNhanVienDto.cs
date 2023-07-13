using Abp.Application.Services.Dto;
using System;

namespace BanHangBeautify.NhanSu.NhanVien_DichVu.Dto
{
    public class DichVuNhanVienDto : EntityDto<Guid>
    {
        public Guid IdNhanVien { get; set; }
        public Guid IdDichVu { get; set; }
    }
}
