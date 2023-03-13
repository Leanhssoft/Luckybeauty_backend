using Abp.Application.Services.Dto;

namespace BanHangBeautify.Roles.Dto
{
    public class PagedRoleResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}

