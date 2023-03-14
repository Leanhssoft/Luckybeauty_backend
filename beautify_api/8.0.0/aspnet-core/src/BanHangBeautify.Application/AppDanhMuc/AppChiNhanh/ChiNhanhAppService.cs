using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.AppDanhMuc.AppChiNhanh.Dto;
using BanHangBeautify.Authorization;
using BanHangBeautify.Entities;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ListResultDto<DM_ChiNhanh>> GetAllChiNhanh()
        {
            ListResultDto<DM_ChiNhanh> result = new ListResultDto<DM_ChiNhanh>();
            var chiNhanhs = await _chiNhanhRepository.GetAllListAsync();
            result.Items = chiNhanhs.Where(x => x.IsDeleted == false).ToList();
            return result;
        }
        [HttpPost]
        public async Task CreateOrEditChiNhanh(CreateChiNhanhDto dto)
        {
            var exits = await _chiNhanhRepository.FirstOrDefaultAsync(dto.Id);
            if (exits == null)
            {
                await Create(dto);
            }
            else await Edit(dto, exits);
        }
        [NonAction]
        public async Task Create(CreateChiNhanhDto dto)
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
            await _chiNhanhRepository.InsertAsync(chiNhanh);
            //return ObjectMapper.Map<ChiNhanhDto>(dto);
        }
        [NonAction]
        public async Task Edit(CreateChiNhanhDto dto, DM_ChiNhanh chiNhanh)
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
            await _chiNhanhRepository.UpdateAsync(chiNhanh);
            //return ObjectMapper.Map<ChiNhanhDto>(dto);
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
