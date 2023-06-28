using Abp.Application.Services.Dto;
using System;

namespace BanHangBeautify.NhanSu.NhanVien_DichVu.Dto
{
    public class CreateOrUpdateDichVuNhanVienDto : EntityDto<Guid>
    {
        public Guid IdNhanVien { get; set; }
        public Guid IdHangHoa { get; set; }
    }
}
