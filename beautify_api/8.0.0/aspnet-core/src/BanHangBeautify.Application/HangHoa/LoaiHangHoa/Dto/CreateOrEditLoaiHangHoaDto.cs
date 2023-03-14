using Abp.Application.Services.Dto;
using System;

namespace BanHangBeautify.HangHoa.LoaiHangHoa.Dto
{
    public class CreateOrEditLoaiHangHoaDto : EntityDto<Guid>
    {
        public string MaLoai { get; set; }
        public string TenLoai { get; set; }
        public int TrangThai { get; set; }
    }
}
