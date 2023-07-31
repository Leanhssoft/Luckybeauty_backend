using Abp.Application.Services.Dto;
using BanHangBeautify.NhanSu.NhanVien.Dto;
using System.Threading.Tasks;

namespace BanHangBeautify.NhanSu.NhanVien.Responsitory
{
    public interface INhanSuRepository
    {
        Task<PagedResultDto<NhanSuItemDto>> GetAllNhanSu(PagedNhanSuRequestDto input);
    }
}
