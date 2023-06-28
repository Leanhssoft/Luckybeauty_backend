using Abp.Application.Services.Dto;

namespace BanHangBeautify.PhongBan.Dto
{
    public class PhongBanPagedResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}
