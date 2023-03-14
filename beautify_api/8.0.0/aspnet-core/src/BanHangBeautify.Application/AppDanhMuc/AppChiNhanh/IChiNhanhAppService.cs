using Abp.Application.Services.Dto;
using BanHangBeautify.AppDanhMuc.AppChiNhanh.Dto;
using BanHangBeautify.Entities;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDanhMuc.AppChiNhanh
{
    public interface IChiNhanhAppService
    {
        public Task<ChiNhanhDto> CreateOrEditChiNhanh(CreateChiNhanhDto dto);
        public Task<ListResultDto<DM_ChiNhanh>> GetAllChiNhanh();
    }
}
