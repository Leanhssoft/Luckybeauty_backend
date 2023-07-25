using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.AppDanhMuc.ViTriPhong.Dto;
using BanHangBeautify.Authorization;
using BanHangBeautify.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDanhMuc.ViTriPhong
{
    [AbpAuthorize(PermissionNames.Pages_ViTriPhong)]
    public class ViTriPhongAppService : SPAAppServiceBase
    {
        private readonly IRepository<DM_ViTriPhong, Guid> _repository;
        public ViTriPhongAppService(IRepository<DM_ViTriPhong, Guid> repository)
        {
            _repository = repository;
        }
        public async Task<ViTriPhongDto> CreateOrEdit(CreateOrEditViTriPhongDto input)
        {
            var checkExist = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (checkExist != null)
            {
                return await Update(input, checkExist);
            }
            return await Create(input);
        }
        [NonAction]
        public async Task<ViTriPhongDto> Create(CreateOrEditViTriPhongDto input)
        {
            ViTriPhongDto result = new ViTriPhongDto();
            DM_ViTriPhong data = new DM_ViTriPhong();
            data = ObjectMapper.Map<DM_ViTriPhong>(input);
            data.Id = Guid.NewGuid();
            data.CreatorUserId = AbpSession.UserId;
            data.CreationTime = DateTime.Now;
            data.TenantId = AbpSession.TenantId ?? 1;
            data.IsDeleted = false;
            await _repository.InsertAsync(data);
            result = ObjectMapper.Map<ViTriPhongDto>(input);
            return result;
        }
        [NonAction]
        public async Task<ViTriPhongDto> Update(CreateOrEditViTriPhongDto input, DM_ViTriPhong oldData)
        {
            ViTriPhongDto result = new ViTriPhongDto();
            oldData.MaViTriPhong = input.MaViTriPhong;
            oldData.TenViTriPhong = input.TenViTriPhong;
            oldData.LastModificationTime = DateTime.Now;
            oldData.LastModifierUserId = AbpSession.UserId;
            await _repository.UpdateAsync(oldData);
            result = ObjectMapper.Map<ViTriPhongDto>(oldData);
            return result;
        }
        [HttpPost]
        public async Task<ViTriPhongDto> Delete(Guid id)
        {
            var data = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                data.IsDeleted = true;
                data.DeleterUserId = AbpSession.UserId;
                data.DeletionTime = DateTime.Now;
                await _repository.UpdateAsync(data);
                return ObjectMapper.Map<ViTriPhongDto>(data);
            }
            return new ViTriPhongDto();
        }
        public async Task<CreateOrEditViTriPhongDto> GetForEdit(Guid id)
        {
            var data = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                return ObjectMapper.Map<CreateOrEditViTriPhongDto>(data);
            }
            return new CreateOrEditViTriPhongDto();
        }
        public async Task<PagedResultDto<ViTriPhongDto>> GetAll(PagedRequestDto input)
        {
            PagedResultDto<ViTriPhongDto> result = new PagedResultDto<ViTriPhongDto>();
            input.Keyword = string.IsNullOrEmpty(input.Keyword) ? "" : input.Keyword;
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            var lstData = await _repository.GetAll().Where(x => x.IsDeleted == false && x.TenantId == (AbpSession.TenantId)).OrderByDescending(x => x.CreationTime).ToListAsync();
            result.TotalCount = lstData.Count;
            var data = lstData.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            result.Items = ObjectMapper.Map<List<ViTriPhongDto>>(data);
            return result;
        }
    }
}
