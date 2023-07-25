using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Entities;
using BanHangBeautify.Quy.KhoanThuChi.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.Quy.KhoanThuChi
{
    [AbpAuthorize(PermissionNames.Pages_KhoanThuChi)]
    public class KhoanThuChiAppService : SPAAppServiceBase
    {
        private readonly IRepository<DM_KhoanThuChi, Guid> _khoanThuChiRepository;
        public KhoanThuChiAppService(IRepository<DM_KhoanThuChi, Guid> khoanThuChiRepository)
        {
            _khoanThuChiRepository = khoanThuChiRepository;
        }
        [AbpAuthorize(PermissionNames.Pages_KhoanThuChi_Create, PermissionNames.Pages_KhoanThuChi_Edit)]
        public async Task<KhoanThuChiDto> CreateOrEdit(CreateOrEditKhoanThuChiDto input)
        {
            var checkExist = await _khoanThuChiRepository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (checkExist != null)
            {
                return await Update(input, checkExist);
            }
            return await Create(input);
        }
        [NonAction]
        public async Task<KhoanThuChiDto> Create(CreateOrEditKhoanThuChiDto input)
        {
            KhoanThuChiDto result = new KhoanThuChiDto();
            DM_KhoanThuChi data = new DM_KhoanThuChi();
            data = ObjectMapper.Map<DM_KhoanThuChi>(input);
            data.Id = Guid.NewGuid();
            data.CreationTime = DateTime.Now;
            data.CreatorUserId = AbpSession.UserId;
            data.TenantId = AbpSession.TenantId ?? 1;
            data.IsDeleted = false;
            await _khoanThuChiRepository.InsertAsync(data);
            return result;
        }
        [NonAction]
        public async Task<KhoanThuChiDto> Update(CreateOrEditKhoanThuChiDto input, DM_KhoanThuChi oldData)
        {
            KhoanThuChiDto result = new KhoanThuChiDto();
            oldData.LaKhoanThu = input.LaKhoanThu;
            oldData.MaKhoanThuChi = input.MaKhoanThuChi;
            oldData.TenKhoanThuChi = input.TenKhoanThuChi;
            oldData.ChungTuApDung = input.ChungTuApDung;
            oldData.GhiChu = input.GhiChu;
            oldData.LastModificationTime = DateTime.Now;
            oldData.LastModifierUserId = AbpSession.UserId;
            await _khoanThuChiRepository.UpdateAsync(oldData);
            return result;
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_KhoanThuChi_Delete)]
        public async Task<KhoanThuChiDto> Delete(Guid id)
        {
            var data = await _khoanThuChiRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                data.IsDeleted = true;
                data.DeleterUserId = AbpSession.UserId;
                data.DeletionTime = DateTime.Now;
                await _khoanThuChiRepository.UpdateAsync(data);
                return ObjectMapper.Map<KhoanThuChiDto>(data);
            }
            return new KhoanThuChiDto();
        }
        public async Task<CreateOrEditKhoanThuChiDto> GetForEdit(Guid id)
        {
            var data = await _khoanThuChiRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                return ObjectMapper.Map<CreateOrEditKhoanThuChiDto>(data);
            }
            return new CreateOrEditKhoanThuChiDto();
        }
        public async Task<PagedResultDto<KhoanThuChiDto>> GetAll(PagedRequestDto input)
        {
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            input.Keyword = string.IsNullOrEmpty(input.Keyword) ? "" : input.Keyword;
            PagedResultDto<KhoanThuChiDto> result = new PagedResultDto<KhoanThuChiDto>();
            var listData = await _khoanThuChiRepository.GetAll().Where(x => x.IsDeleted == false && x.TenantId == (AbpSession.TenantId ?? 1)).ToListAsync();
            result.TotalCount = listData.Count;
            listData = listData.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            result.Items = ObjectMapper.Map<List<KhoanThuChiDto>>(listData);
            return result;
        }
    }
}
