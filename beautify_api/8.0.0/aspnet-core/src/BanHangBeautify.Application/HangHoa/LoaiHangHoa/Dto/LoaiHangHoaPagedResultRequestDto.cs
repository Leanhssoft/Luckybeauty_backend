using Abp.Application.Services.Dto;

namespace BanHangBeautify.HangHoa.LoaiHangHoa.Dto
{
    public class LoaiHangHoaPagedResultRequestDto : PagedResultRequestDto
    {
        public int? TenantId { get; set; }
        public string Keyword { get; set; }
    }
}
