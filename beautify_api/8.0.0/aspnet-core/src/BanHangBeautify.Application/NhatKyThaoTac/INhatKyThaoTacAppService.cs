using Abp.Application.Services.Dto;
using BanHangBeautify.NhatKyHoatDong.Dto;
using BanHangBeautify.NhatKyThaoTac.Dto;
using System;
using System.Threading.Tasks;

namespace BanHangBeautify.NhatKyHoatDong
{
    public interface INhatKyThaoTacAppService
    {
        public Task<NhatKyThaoTacDto> CreateNhatKyHoatDong(CreateNhatKyThaoTacDto input);
        public Task<NhatKyThaoTacItemDto> GetDetail(Guid id);
        public Task<PagedResultDto<NhatKyThaoTacItemDto>> GetAll(PagedNhatKyRequestDto input);
        public Task<NhatKyThaoTacDto> Delete(Guid id);
    }
}
