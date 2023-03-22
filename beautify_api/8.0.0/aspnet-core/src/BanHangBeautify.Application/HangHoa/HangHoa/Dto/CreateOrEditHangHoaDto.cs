using Abp.Application.Services.Dto;
using System;

namespace BanHangBeautify.HangHoa.HangHoa.Dto
{
    public class CreateOrEditHangHoaDto : EntityDto<Guid>
    {
        public string MaHangHoa { get; set; }
        public string TenHangHoa { get; set; }
        public int TrangThai { get; set; }
        public Guid IdLoaiHangHoa { get; set; }
        public string MoTa { get; set; }
    }
}
