﻿using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Entities;
using BanHangBeautify.NhanSu.ChucVu.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.NhanSu.ChucVu
{
    [AbpAuthorize(PermissionNames.Pages_ChucVu)]
    public class ChucVuAppService : SPAAppServiceBase
    {
        private readonly IRepository<NS_ChucVu, Guid> _repository;
        public ChucVuAppService(IRepository<NS_ChucVu, Guid> repository)
        {
            _repository = repository;
        }
        public async Task<ChucVuDto> CreateOrEdit(CreateOrEditChucVuDto dto)
        {
            try
            {
                var find = await _repository.FirstOrDefaultAsync(x => x.Id == dto.Id);
                if (find == null)
                {
                    return await Create(dto);
                }
                else
                {
                    return await Edit(dto, find);
                }
            }
            catch (Exception)
            {
                return new ChucVuDto();
            }
        }
        [NonAction]
        public async Task<ChucVuDto> Create(CreateOrEditChucVuDto dto)
        {
            NS_ChucVu chucVu = new NS_ChucVu();
            chucVu.Id = Guid.NewGuid();
            chucVu.MaChucVu = dto.MaChucVu;
            chucVu.TenChucVu = dto.TenChucVu;
            chucVu.TrangThai = 0;
            chucVu.MoTa = dto.MoTa;
            chucVu.TenantId = AbpSession.TenantId ?? 1;
            chucVu.CreatorUserId = AbpSession.UserId;
            var result = ObjectMapper.Map<ChucVuDto>(chucVu);
            await _repository.InsertAsync(chucVu);
            return result;
        }
        [NonAction]
        public async Task<ChucVuDto> Edit(CreateOrEditChucVuDto dto, NS_ChucVu chucVu)
        {
            chucVu.MaChucVu = dto.MaChucVu;
            chucVu.TenChucVu = dto.TenChucVu;
            chucVu.TrangThai = dto.TrangThai;
            chucVu.MoTa = dto.MoTa;
            chucVu.LastModifierUserId = AbpSession.UserId;
            chucVu.LastModificationTime = DateTime.Now;
            var result = ObjectMapper.Map<ChucVuDto>(chucVu);
            await _repository.InsertAsync(chucVu);
            return result;
        }
        [HttpPost]
        public async Task<ChucVuDto> Delete(Guid Id)
        {
            var find = await _repository.FirstOrDefaultAsync(x => x.Id == Id);
            if (find != null)
            {
                find.IsDeleted = true;
                find.DeleterUserId = AbpSession.UserId;
                find.DeletionTime = DateTime.Now;
                find.TrangThai = 1;
                var result = ObjectMapper.Map<ChucVuDto>(find);
                await _repository.UpdateAsync(find);
                return result;
            }
            return new ChucVuDto();
        }
        public async Task<NS_ChucVu> GetDetail(Guid id)
        {
            return await _repository.GetAsync(id);
        }
        public async Task<PagedResultDto<NS_ChucVu>> GetAll(PagedResultRequestDto input, string keyWord)
        {
            PagedResultDto<NS_ChucVu> result = new PagedResultDto<NS_ChucVu>();
            try
            {
                var lstChucVu = await _repository.GetAll().Where(x => x.TenantId == (AbpSession.TenantId??1) && x.IsDeleted == false).OrderByDescending(x => x.CreationTime).ToListAsync();
                result.TotalCount = lstChucVu.Count;
                if (!string.IsNullOrEmpty(keyWord))
                {
                    lstChucVu = lstChucVu.Where(x => x.TenChucVu.Contains(keyWord) || x.MaChucVu.Contains(keyWord)).ToList();
                }
                input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
                result.Items = lstChucVu.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            }
            catch (Exception)
            {
                result.Items = null;
                result.TotalCount=0;
            }

            return result;
        }
    }
}
