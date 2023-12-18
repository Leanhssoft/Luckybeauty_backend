using Abp.Application.Services.Dto;
using BanHangBeautify.ChietKhau.ChietKhauDichVu.Dto;
using System;
using System.Threading.Tasks;

namespace BanHangBeautify.ChietKhau.ChietKhauDichVu.Repository
{
    public interface IChietKhauDichVuRepository
    {
        public Task<PagedResultDto<ChietKhauDichVuItemDto>> GetAll(PagedRequestDto input, int tenantId, Guid? idNhanVien= null, Guid? idChiNhanh = null);
        Task<PagedResultDto<ChietKhauDichVuItemDto_TachRiengCot>> GetAllSetup_HoaHongDichVu(PagedRequestDto input, int tenantId,
            Guid? idNhanVien = null, Guid? idChiNhanh = null);
        Task<int> AddMultiple_ChietKhauDichVu_toMultipleNhanVien(ChietKhauDichVuDto_AddMultiple param, int tenantId);
    }
}
