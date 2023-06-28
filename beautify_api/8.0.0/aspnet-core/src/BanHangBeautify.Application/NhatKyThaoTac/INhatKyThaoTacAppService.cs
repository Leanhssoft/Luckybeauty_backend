using Abp.Application.Services.Dto;
using BanHangBeautify.NhatKyHoatDong.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.NhatKyHoatDong
{
    public interface INhatKyThaoTacAppService
    {
        public Task<NhatKyThaoTacDto> CreateNhatKyHoatDong(CreateNhatKyThaoTacDto input);
        public Task<NhatKyThaoTacItemDto> GetDetail(Guid id);
        public Task<PagedResultDto<NhatKyThaoTacItemDto>> GetAll(PagedRequestDto input);
        public Task<NhatKyThaoTacDto> Delete(Guid id);
    }
}
