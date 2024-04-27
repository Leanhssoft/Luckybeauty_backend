using Abp.Application.Services.Dto;
using BanHangBeautify.BaoCao.BaoCaoLichHen.Dto;
using System.Threading.Tasks;

namespace BanHangBeautify.BaoCao.BaoCaoLichHen.Respository
{
    public interface IBaoCaoLichHenRepository
    {
        public Task<PagedResultDto<BaoCaoLichHenDto>> GetBaoCaoLichHen(PagedBaoCaoLichHenRequestDto input, int tenantId);
        Task<PagedResultDto<BaoCaoKhachHangCheckInDto>> GetBaoCaoKhachHang_CheckIn(ParamSearchBaoCaoCheckin input);
    }
}
