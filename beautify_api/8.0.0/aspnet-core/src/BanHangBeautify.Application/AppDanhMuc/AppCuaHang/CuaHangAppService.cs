using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.AppDanhMuc.AppCuaHang.Dto;
using BanHangBeautify.Authorization;
using BanHangBeautify.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDanhMuc.AppCuaHang
{
    [AbpAuthorize(PermissionNames.Pages_CongTy)]
    public class CuaHangAppService : SPAAppServiceBase//, ICuaHangAppService
    {
        private readonly IRepository<HT_CongTy, Guid> _congTyRepository;
        private readonly IRepository<DM_ChiNhanh, Guid> _chiNhanhRepository;
        public CuaHangAppService(IRepository<HT_CongTy, Guid> congTyRepository, IRepository<DM_ChiNhanh, Guid> chiNhanhRepository)
        {
            _congTyRepository = congTyRepository;
            _chiNhanhRepository = chiNhanhRepository;
        }
        public async Task CreateCongTy(CreateOrEditCuaHangDto dto)
        {
            var findExist = await _congTyRepository.FirstOrDefaultAsync(dto.Id);
            if (findExist == null)
            {
                await CreateCuaHang(dto);
            }
            else
            {
                await EditCuaHang(dto, findExist);
            }
        }
        [NonAction]
        public async Task CreateCuaHang(CreateOrEditCuaHangDto dto)
        {
            HT_CongTy data = new HT_CongTy();
            data.Id = Guid.NewGuid();
            data.TenCongTy = dto.TenCongTy;
            data.DiaChi = dto.DiaChi;
            data.GhiChu = dto.GhiChu;
            data.MaSoThue = dto.MaSoThue;
            data.Logo = dto.Logo;
            data.TenantId = AbpSession.TenantId ?? 1;
            data.CreatorUserId = AbpSession.UserId;
            data.CreationTime = DateTime.Now;
            data.NgayTao = DateTime.Now;
            await _congTyRepository.InsertAsync(data);
            DM_ChiNhanh chiNhanh = new DM_ChiNhanh();
            chiNhanh.Id = Guid.NewGuid();
            chiNhanh.MaChiNhanh = dto.MaChiNhanh;
            chiNhanh.TenChiNhanh = dto.TenChiNhanh;
            chiNhanh.MaSoThue = dto.MaSoThue;
            chiNhanh.DiaChi = dto.DiaChi;
            chiNhanh.GhiChu = dto.GhiChu;
            chiNhanh.Logo = dto.Logo;
            chiNhanh.NgayApDung = dto.NgayApDung ?? DateTime.Now;
            chiNhanh.NgayHetHan = dto.NgayHetHan ?? DateTime.Now.AddDays(7);
            chiNhanh.TenantId = AbpSession.TenantId ?? 1;
            chiNhanh.CreatorUserId = AbpSession.UserId;
            chiNhanh.NgayTao = DateTime.Now;
            chiNhanh.IdCongTy = data.Id;
            chiNhanh.CreationTime = DateTime.Now;
            await _chiNhanhRepository.InsertAsync(chiNhanh);
            //CuaHangDto store = new CuaHangDto();
            //store = ObjectMapper.Map<CuaHangDto>(dto);
            //return store;
        }
        [NonAction]
        public async Task EditCuaHang(CreateOrEditCuaHangDto dto, HT_CongTy item)
        {
            item.TenCongTy = dto.TenCongTy;
            item.DiaChi = dto.DiaChi;
            item.GhiChu = dto.GhiChu;
            item.MaSoThue = dto.MaSoThue;
            item.Logo = dto.Logo;
            item.TenantId = AbpSession.TenantId ?? 1;
            item.CreatorUserId = AbpSession.UserId;
            item.CreationTime = DateTime.Now;
            item.NgayTao = DateTime.Now;
            await _congTyRepository.UpdateAsync(item);
            //CuaHangDto store = new CuaHangDto();
            //store = ObjectMapper.Map<CuaHangDto>(dto);
            //return store;
        }

        [AbpAuthorize(PermissionNames.Pages_CongTy_Delete)]
        public async Task<bool> DeleteCongTy(Guid id)
        {
            bool result = false;
            var congTy = await _congTyRepository.GetAsync(id);
            if (congTy.Id == id)
            {
                congTy.IsDeleted = true;
                await _congTyRepository.UpdateAsync(congTy);
                result = true;
            }
            return result;
        }
        public async Task<ListResultDto<HT_CongTy>> GetAllCongTy()
        {
            ListResultDto<HT_CongTy> result = new ListResultDto<HT_CongTy>();
            var congTys = await _congTyRepository.GetAllListAsync();
            result.Items = congTys.Where(x => x.IsDeleted == false).ToList();
            return result;
        }
    }
}
