using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.AppDanhMuc.AppCuaHang.Dto;
using BanHangBeautify.Authorization;
using BanHangBeautify.Consts;
using BanHangBeautify.Entities;
using BanHangBeautify.NhatKyHoatDong;
using BanHangBeautify.NhatKyHoatDong.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDanhMuc.AppCuaHang
{
    [AbpAuthorize]
    public class CuaHangAppService : SPAAppServiceBase
    {
        private readonly IRepository<HT_CongTy, Guid> _congTyRepository;
        private readonly IRepository<DM_ChiNhanh, Guid> _chiNhanhRepository;
        INhatKyThaoTacAppService _audilogService;
        public CuaHangAppService(IRepository<HT_CongTy, Guid> congTyRepository, IRepository<DM_ChiNhanh, Guid> chiNhanhRepository,INhatKyThaoTacAppService audilogervice)
        {
            _congTyRepository = congTyRepository;
            _chiNhanhRepository = chiNhanhRepository;
            _audilogService = audilogervice;
        }

        [AbpAuthorize(PermissionNames.Pages_CongTy_Create)]
        public async Task<CuaHangDto> CreateCuaHang(CreateCuaHangDto dto)
        {
            HT_CongTy data = new HT_CongTy();
            data.Id = Guid.NewGuid();
            data.TenCongTy = dto.TenCongTy;
            data.DiaChi = dto.DiaChi;
            data.GhiChu = dto.GhiChu;
            data.SoDienThoai = dto.SoDienThoai;
            data.MaSoThue = dto.MaSoThue;
            data.Logo = dto.Logo;
            data.Website = dto.Website;
            data.Facebook = dto.Facebook;
            data.Twitter = dto.Twitter;
            data.Instagram = dto.Instagram;
            data.TenantId = AbpSession.TenantId ?? 1;
            data.CreatorUserId = AbpSession.UserId;
            data.CreationTime = DateTime.Now;
            await _congTyRepository.InsertAsync(data);
            DM_ChiNhanh chiNhanh = new DM_ChiNhanh();
            chiNhanh.Id = Guid.NewGuid();
            chiNhanh.MaChiNhanh = dto.MaChiNhanh;
            chiNhanh.TenChiNhanh = dto.TenChiNhanh;
            chiNhanh.SoDienThoai = dto.SoDienThoai;
            chiNhanh.MaSoThue = dto.MaSoThue;
            chiNhanh.DiaChi = dto.DiaChi;
            chiNhanh.GhiChu = dto.GhiChu;
            chiNhanh.Logo = dto.Logo;
            chiNhanh.NgayApDung = dto.NgayApDung ?? DateTime.Now;
            chiNhanh.NgayHetHan = dto.NgayHetHan ?? DateTime.Now.AddDays(7);
            chiNhanh.TenantId = AbpSession.TenantId ?? 1;
            chiNhanh.CreatorUserId = AbpSession.UserId;
            chiNhanh.IdCongTy = data.Id;
            chiNhanh.CreationTime = DateTime.Now;
            var result = ObjectMapper.Map<CuaHangDto>(data);
            await _chiNhanhRepository.InsertAsync(chiNhanh);
            return result;
            //CuaHangDto store = new CuaHangDto();
            //store = ObjectMapper.Map<CuaHangDto>(dto);
            //return store;
        }
        [AbpAuthorize(PermissionNames.Pages_CongTy_Edit)]
        public async Task<CuaHangDto> EditCuaHang(EditCuaHangDto dto)
        {
            CuaHangDto store = new CuaHangDto();
            var item = await _congTyRepository.FirstOrDefaultAsync(dto.Id);
            if (item != null)
            {
                item.TenCongTy = dto.TenCongTy;
                item.DiaChi = dto.DiaChi;
                item.SoDienThoai = dto.SoDienThoai;
                item.GhiChu = dto.GhiChu;
                item.MaSoThue = dto.MaSoThue;
                item.Logo = dto.Logo;
                item.Website = dto.Website;
                item.Facebook = dto.Facebook;
                item.Twitter = dto.Twitter;
                item.Instagram = dto.Instagram;
                item.LastModifierUserId = AbpSession.UserId;
                item.LastModificationTime = DateTime.Now;
                store = ObjectMapper.Map<CuaHangDto>(item);
                await _congTyRepository.UpdateAsync(item);
                var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
                nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Update;
                nhatKyThaoTacDto.ChucNang = "Cửa hàng";
                nhatKyThaoTacDto.NoiDung = "Cập nhật thông tin chi nhánh";
                nhatKyThaoTacDto.NoiDungChiTiet = "Cập nhật thông tin chi nhánh";
                await _audilogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
            }
            return store;
        }

        [AbpAuthorize(PermissionNames.Pages_CongTy_Delete)]
        [HttpPost]
        public async Task<bool> DeleteCongTy(Guid id)
        {
            bool result = false;
            var congTy = await _congTyRepository.GetAsync(id);
            if (congTy.Id == id)
            {
                congTy.IsDeleted = true;
                await _congTyRepository.UpdateAsync(congTy);
                var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
                nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Delete;
                nhatKyThaoTacDto.ChucNang = "Cửa hàng";
                nhatKyThaoTacDto.NoiDung = "Xóa của hàng";
                nhatKyThaoTacDto.NoiDungChiTiet = "Xóa cửa hàng";
                result = true;
            }
            return result;
        }
        public async Task<HT_CongTy> GetCongTy(Guid id)
        {
            return await _congTyRepository.GetAsync(id);
        }
        public async Task<EditCuaHangDto> GetCongTyForEdit(Guid idChiNhanh)
        {
            var chiNhanh = _chiNhanhRepository.FirstOrDefault(x => x.Id == idChiNhanh);
            if (chiNhanh != null)
            {
                var cuaHang = await _congTyRepository.GetAsync(chiNhanh.IdCongTy);
                return new EditCuaHangDto()
                {
                    Id = cuaHang.Id,
                    DiaChi = cuaHang.DiaChi,
                    Facebook = cuaHang.Facebook,
                    GhiChu = cuaHang.GhiChu,
                    Instagram = cuaHang.Instagram,
                    Logo = cuaHang.Logo,
                    MaSoThue = cuaHang.MaSoThue,
                    SoDienThoai = cuaHang.SoDienThoai,
                    TenCongTy = cuaHang.TenCongTy,
                    Twitter = cuaHang.Twitter,
                    Website = cuaHang.Website
                };
            }
            return new EditCuaHangDto();
        }
        public async Task<ListResultDto<HT_CongTy>> GetAllCongTy(PagedResultRequestDto input, string keyWord)
        {
            ListResultDto<HT_CongTy> result = new ListResultDto<HT_CongTy>();
            var congTys = await _congTyRepository.GetAll().Where(x => x.IsDeleted == false && x.TenantId == (AbpSession.TenantId ?? 1)).OrderByDescending(x => x.CreationTime).ToListAsync();
            if (!string.IsNullOrEmpty(keyWord))
            {
                congTys = congTys.Where(x => x.MaSoThue.Contains(keyWord) || x.TenCongTy.Contains(keyWord) || x.DiaChi.Contains(keyWord) || x.SoDienThoai.Contains(keyWord)).ToList();
            }
            input.MaxResultCount = 10;
            input.SkipCount = input.SkipCount > 0 ? input.SkipCount * 10 : 0;
            result.Items = congTys.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            return result;
        }
    }
}
