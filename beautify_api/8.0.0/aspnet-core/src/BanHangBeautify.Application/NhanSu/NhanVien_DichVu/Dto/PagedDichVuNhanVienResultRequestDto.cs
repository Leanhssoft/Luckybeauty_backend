using Abp.Application.Services.Dto;

namespace BanHangBeautify.NhanSu.NhanVien_DichVu.Dto
{
    public class PagedDichVuNhanVienResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
        public bool? IsActive { get; set; }
    }
}
