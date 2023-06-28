using Abp.Application.Services.Dto;

namespace BanHangBeautify.KhachHang.LoaiKhach.Dto
{
    public class PagedLoaiKhachResultRequestDto : PagedResultRequestDto
    {
        public string? Keyword { set; get; }
    }
}
