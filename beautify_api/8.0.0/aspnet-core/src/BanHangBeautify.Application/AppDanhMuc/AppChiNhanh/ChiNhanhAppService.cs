using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.AppDanhMuc.AppChiNhanh.Dto;
using BanHangBeautify.Authorization;
using BanHangBeautify.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDanhMuc.AppChiNhanh
{
    [AbpAuthorize(PermissionNames.Pages_ChiNhanh)]
    public class ChiNhanhAppService : SPAAppServiceBase
    {
        public readonly IRepository<DM_ChiNhanh, Guid> _chiNhanhRepository;
        public ChiNhanhAppService(IRepository<DM_ChiNhanh, Guid> chiNhanhRepository)
        {
            _chiNhanhRepository = chiNhanhRepository;
        }
        [HttpGet]
        public async Task<ListResultDto<DM_ChiNhanh>> GetAllChiNhanh(PagedResultRequestDto input, string keyWord)
        {
            ListResultDto<DM_ChiNhanh> result = new ListResultDto<DM_ChiNhanh>();
            var chiNhanhs = await _chiNhanhRepository.GetAll().Where(x => x.IsDeleted == false && x.TenantId == (AbpSession.TenantId ?? 1)).OrderByDescending(x => x.CreationTime).ToListAsync();
            if (!string.IsNullOrEmpty(keyWord))
            {
                chiNhanhs = chiNhanhs.Where(x => x.MaSoThue.Contains(keyWord) || x.TenChiNhanh.Contains(keyWord) || x.DiaChi.Contains(keyWord) || x.SoDienThoai.Contains(keyWord)).ToList();
            }
            input.MaxResultCount = 10;
            input.SkipCount = input.SkipCount > 0 ? input.SkipCount * 10 : 0;
            result.Items = chiNhanhs.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            return result;
        }
        public async Task<DM_ChiNhanh> GetChiNhanh(Guid id)
        {
            return await _chiNhanhRepository.GetAsync(id);
        }
        [HttpPost]
        public async Task<ChiNhanhDto> CreateOrEditChiNhanh(CreateChiNhanhDto dto)
        {
            var exits = await _chiNhanhRepository.FirstOrDefaultAsync(dto.Id);
            if (exits == null)
            {
                return await Create(dto);
            }
            else return await Edit(dto, exits);
        }
        [NonAction]
        public async Task<ChiNhanhDto> Create(CreateChiNhanhDto dto)
        {
            DM_ChiNhanh chiNhanh = new DM_ChiNhanh();
            chiNhanh.Id = Guid.NewGuid();
            chiNhanh.MaChiNhanh = dto.MaChiNhanh;
            chiNhanh.TenChiNhanh = dto.TenChiNhanh;
            chiNhanh.MaSoThue = dto.MaSoThue;
            chiNhanh.DiaChi = dto.DiaChi;
            chiNhanh.GhiChu = dto.GhiChu;
            chiNhanh.Logo = dto.Logo;
            chiNhanh.NgayApDung = dto.NgayApDung;
            chiNhanh.NgayHetHan = dto.NgayHetHan;
            chiNhanh.TenantId = AbpSession.TenantId ?? 1;
            chiNhanh.CreatorUserId = AbpSession.UserId;
            chiNhanh.NgayTao = DateTime.Now;
            chiNhanh.IdCongTy = dto.IdCongTy;
            chiNhanh.CreationTime = DateTime.Now;
            var result = ObjectMapper.Map<ChiNhanhDto>(chiNhanh);
            await _chiNhanhRepository.InsertAsync(chiNhanh);
            return result;
            //return ObjectMapper.Map<ChiNhanhDto>(dto);
        }
        [NonAction]
        public async Task<ChiNhanhDto> Edit(CreateChiNhanhDto dto, DM_ChiNhanh chiNhanh)
        {
            chiNhanh.MaChiNhanh = dto.MaChiNhanh;
            chiNhanh.TenChiNhanh = dto.TenChiNhanh;
            chiNhanh.MaSoThue = dto.MaSoThue;
            chiNhanh.DiaChi = dto.DiaChi;
            chiNhanh.GhiChu = dto.GhiChu;
            chiNhanh.Logo = dto.Logo;
            chiNhanh.NgayApDung = dto.NgayApDung;
            chiNhanh.NgayHetHan = dto.NgayHetHan;
            chiNhanh.TenantId = AbpSession.TenantId ?? 1;
            chiNhanh.LastModifierUserId = AbpSession.UserId;
            chiNhanh.NgaySua = DateTime.Now;
            var result = ObjectMapper.Map<ChiNhanhDto>(chiNhanh);
            await _chiNhanhRepository.UpdateAsync(chiNhanh);
            return result;
        }
        [HttpPost]
        public async Task<bool> DeleteChiNhanh(Guid Id)
        {
            bool result = false;
            var findBranch = await _chiNhanhRepository.FirstOrDefaultAsync(x => x.Id == Id);
            if (findBranch != null)
            {
                findBranch.IsDeleted = true;
                findBranch.DeleterUserId = AbpSession.UserId;
                findBranch.DeletionTime = DateTime.Now;
                _chiNhanhRepository.Update(findBranch);
                result = true;
            }
            return result;
        }
    }
}
