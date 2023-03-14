using Abp.Application.Services.Dto;
using BanHangBeautify.AppDanhMuc.AppCuaHang.Dto;
using BanHangBeautify.Entities;
using System;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDanhMuc.AppCuaHang
{
    public interface ICuaHangAppService
    {
        public Task<CuaHangDto> CreateCongTy(CreateOrEditCuaHangDto dto);
        public Task<bool> DeleteCongTy(Guid id);
        public Task<ListResultDto<HT_CongTy>> GetAllCongTy();
    }
}
