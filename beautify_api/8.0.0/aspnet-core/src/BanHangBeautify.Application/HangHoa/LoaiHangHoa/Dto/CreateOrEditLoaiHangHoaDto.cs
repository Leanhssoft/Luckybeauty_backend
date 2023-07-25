using Abp.Application.Services.Dto;

namespace BanHangBeautify.HangHoa.LoaiHangHoa.Dto
{
    public class CreateOrEditLoaiHangHoaDto : EntityDto<int>
    {
        public string MaLoai { get; set; }
        public string TenLoai { get; set; }
        public int TrangThai { get; set; }
    }
}
