using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.PhongBan.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.PhongBan
{
    [AbpAuthorize(PermissionNames.Pages_PhongBan)]
    public class PhongBanAppService : SPAAppServiceBase
    {
        private readonly IRepository<DM_PhongBan, Guid> _phongBanRepository;
        public PhongBanAppService(IRepository<DM_PhongBan, Guid> phongBanRepository)
        {
            _phongBanRepository = phongBanRepository;
        }
        [AbpAuthorize(PermissionNames.Pages_PhongBan_Create,PermissionNames.Pages_PhongBan_Edit)]
        public async Task<PhongBanDto> CreateOrEdit(CreateOrEditPhongBanDto dto)
        {
            var phongBan = await _phongBanRepository.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (phongBan == null)
            {
                return await Create(dto);
            }
            else
            {
                return await Edit(dto, phongBan);
            }
        }
        [NonAction]
        public async Task<PhongBanDto> Create(CreateOrEditPhongBanDto dto)
        {
            DM_PhongBan data = new DM_PhongBan();
            data.Id = Guid.NewGuid();
            data.IdChiNhanh = dto.IdChiNhanh;
            data.MaPhongBan = dto.MaPhongBan;
            data.TenPhongBan = dto.TenPhongBan;
            data.TenantId = AbpSession.TenantId ?? 1;
            data.CreatorUserId = AbpSession.UserId;
            data.CreationTime = DateTime.Now;
            var result = ObjectMapper.Map<PhongBanDto>(data);
            await _phongBanRepository.InsertAsync(data);
            return result;
        }
        [NonAction]
        public async Task<PhongBanDto> Edit(CreateOrEditPhongBanDto dto, DM_PhongBan data)
        {
            data.IdChiNhanh = dto.IdChiNhanh;
            data.MaPhongBan = dto.MaPhongBan;
            data.TenPhongBan = dto.TenPhongBan;
            data.LastModifierUserId = AbpSession.UserId;
            data.LastModificationTime = DateTime.Now;
            var result = ObjectMapper.Map<PhongBanDto>(data);
            await _phongBanRepository.UpdateAsync(data);
            return result;
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_PhongBan_Delete)]
        public async Task<PhongBanDto> Delete(Guid id)
        {
            PhongBanDto result = new PhongBanDto();
            var phongBan = await _phongBanRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (phongBan != null)
            {
                phongBan.DeleterUserId = AbpSession.UserId;
                phongBan.DeletionTime = DateTime.Now;
                phongBan.IsDeleted = true;
                result = ObjectMapper.Map<PhongBanDto>(phongBan);
                _phongBanRepository.Update(phongBan);
            }
            return result;
        }
        public async Task<DM_PhongBan> GetPhongBan(Guid id)
        {
            return await _phongBanRepository.GetAsync(id);
        }
        public async Task<PagedResultDto<DM_PhongBan>> GetAll(PhongBanPagedResultRequestDto input)
        {
            PagedResultDto<DM_PhongBan> result = new PagedResultDto<DM_PhongBan>();
            var lstPhongBan = await _phongBanRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId && x.IsDeleted == false).OrderByDescending(x => x.CreationTime).ToListAsync();
            result.TotalCount = lstPhongBan.Count;
            if (!string.IsNullOrEmpty(input.Keyword))
            {
                lstPhongBan = lstPhongBan.Where(x => x.TenPhongBan.Contains(input.Keyword) || x.MaPhongBan.Contains(input.Keyword)).ToList();
            }
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            result.Items = lstPhongBan.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            return result;
        }
    }
}
