using Abp.Application.Services.Dto;

namespace BanHangBeautify.HangHoa.DonViQuiDoi.Dto
{
    public class DonViQuiDoiPagedRequestResultDto : PagedResultRequestDto
    {
        public string Keyword { set; get; }
        public int? TenantId { get; set; }
    }
}
