using Abp.Application.Services.Dto;
using System;

namespace BanHangBeautify.HangHoa.HangHoa.Dto
{
    public class CreateOrEditHangHoaDto : EntityDto<Guid>
    {
        public string TenHangHoa { get; set; }
        public int TrangThai { get; set; } = 1;
        public int IdLoaiHangHoa { get; set; } = 2;
        public string MoTa { get; set; }
    }
}
