using Abp.Application.Services.Dto;
using BanHangBeautify.ChietKhau.ChietKhauDichVu.Dto;
using System;
using System.Threading.Tasks;

namespace BanHangBeautify.ChietKhau.ChietKhauDichVu.Repository
{
    public interface IChietKhauDichVuRepository
    {
        public Task<PagedResultDto<ChietKhauDichVuItemDto>> GetAll(PagedRequestDto input, int tenantId, Guid idNhanVien, Guid idChiNhanh);
    }
}
