﻿using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.PhongBan.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.PhongBan
{
    public class PhongBanAppService : SPAAppServiceBase
    {
        private readonly IRepository<DM_PhongBan, Guid> _phongBanRepository;
        public PhongBanAppService(IRepository<DM_PhongBan, Guid> phongBanRepository)
        {
            _phongBanRepository = phongBanRepository;
        }
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
            data.NgayTao = DateTime.Now;
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
            data.NgaySua = DateTime.Now;
            var result = ObjectMapper.Map<PhongBanDto>(data);
            await _phongBanRepository.UpdateAsync(data);
            return result;
        }
        [HttpPost]
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
        public async Task<ListResultDto<DM_PhongBan>> GetAll(PhongBanPagedResultRequestDto input)
        {
            ListResultDto<DM_PhongBan> result = new ListResultDto<DM_PhongBan>();
            var lstPhongBan = await _phongBanRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId && x.IsDeleted == false).OrderByDescending(x => x.CreationTime).ToListAsync();
            if (!string.IsNullOrEmpty(input.Keyword))
            {
                lstPhongBan = lstPhongBan.Where(x => x.TenPhongBan.Contains(input.Keyword) || x.MaPhongBan.Contains(input.Keyword)).ToList();
            }
            input.MaxResultCount = 10;
            if (input.SkipCount > 0)
            {
                input.SkipCount *= input.MaxResultCount;
            }
            result.Items = lstPhongBan.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            return result;
        }
    }
}
