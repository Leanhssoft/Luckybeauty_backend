using Abp.Application.Services.Dto;

namespace BanHangBeautify.KhachHang.NhomKhach.Dto
{
    public class PagedNhomKhachResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { set; get; }
    }
}