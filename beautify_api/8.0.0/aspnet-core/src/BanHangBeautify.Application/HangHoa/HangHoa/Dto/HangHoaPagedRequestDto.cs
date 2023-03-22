using Abp.Application.Services.Dto;

namespace BanHangBeautify.HangHoa.HangHoa.Dto
{
    public class HangHoaPagedResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
        public int? TenantId { set; get; }
    }
}
