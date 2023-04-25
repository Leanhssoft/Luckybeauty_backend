using Abp.Application.Services.Dto;

namespace BanHangBeautify.KhachHang.KhachHang.Dto
{
    public class PagedKhachHangResultRequestDto : PagedResultRequestDto
    {
        public string keyword { get; set; }
    }
}