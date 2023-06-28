using Abp.Application.Services.Dto;

namespace BanHangBeautify.KhachHang.NguonKhach.Dto
{
    public class PagedNguonKhachResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}