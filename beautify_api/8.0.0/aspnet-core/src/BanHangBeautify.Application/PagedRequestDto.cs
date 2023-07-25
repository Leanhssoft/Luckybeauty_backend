using Abp.Application.Services.Dto;

namespace BanHangBeautify
{
    public class PagedRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
        public string? SortBy { get; set; }
        public string? SortType { get; set; }
    }
}
