using Abp.Application.Services.Dto;
using BanHangBeautify.AppDanhMuc.AppChiNhanh.Dto;
using BanHangBeautify.AppDanhMuc.AppCuaHang.Dto;
using BanHangBeautify.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
