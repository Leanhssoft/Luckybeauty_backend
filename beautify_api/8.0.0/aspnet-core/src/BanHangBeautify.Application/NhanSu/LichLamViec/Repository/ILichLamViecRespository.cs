using Abp.Application.Services.Dto;
using BanHangBeautify.NhanSu.LichLamViec.Dto;
using System.Threading.Tasks;

namespace BanHangBeautify.NhanSu.LichLamViec.Repository
{
    public interface ILichLamViecRespository
    {
        public Task<PagedResultDto<LichLamViecNhanVien>> GetAllLichLamViecWeek(PagedRequestLichLamViecDto input, int tenantId);
    }
}
