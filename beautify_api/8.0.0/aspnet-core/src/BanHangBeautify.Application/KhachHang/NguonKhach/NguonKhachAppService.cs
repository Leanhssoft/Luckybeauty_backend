using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using BanHangBeautify.Authorization;
using BanHangBeautify.Entities;
using BanHangBeautify.KhachHang.NguonKhach.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.KhachHang.NguonKhach
{
    [AbpAuthorize(PermissionNames.Pages_NguonKhach)]
    public class NguonKhachAppService : SPAAppServiceBase
    {
        private IRepository<DM_NguonKhach, Guid> _repository;
        public NguonKhachAppService(IRepository<DM_NguonKhach, Guid> repository)
        {
            _repository = repository;
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_NguonKhach_Create)]
        public async Task<NguonKhachDto> CreateNguonKhach(CreateOrEditNguonKhachDto dto)
        {
            NguonKhachDto result = new NguonKhachDto();
            var nguonKhach = ObjectMapper.Map<DM_NguonKhach>(dto);
            nguonKhach.Id = Guid.NewGuid();
            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var checkMa = _repository.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1)).ToList();
                nguonKhach.MaNguon = "MS00" + (checkMa.Count + 1).ToString();
            }
            nguonKhach.CreationTime = DateTime.Now;
            nguonKhach.CreatorUserId = AbpSession.UserId;
            nguonKhach.TenantId = AbpSession.TenantId ?? 1;
            nguonKhach.IsDeleted = false;
            await _repository.InsertAsync(nguonKhach);
            result = ObjectMapper.Map<NguonKhachDto>(nguonKhach);
            return result;
        }
        [AbpAuthorize(PermissionNames.Pages_NguonKhach_Edit)]
        public async Task<NguonKhachDto> EditNguonKhach(CreateOrEditNguonKhachDto dto)
        {
            NguonKhachDto result = new NguonKhachDto();
            var nguonKhach = ObjectMapper.Map<DM_NguonKhach>(dto);
            nguonKhach.LastModificationTime = DateTime.Now;
            nguonKhach.LastModifierUserId = AbpSession.UserId;
            await _repository.UpdateAsync(nguonKhach);
            result = ObjectMapper.Map<NguonKhachDto>(nguonKhach);

            return result;
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_NguonKhach_Delete)]
        public async Task<NguonKhachDto> Delete(Guid id)
        {
            NguonKhachDto result = new NguonKhachDto();
            var delete = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (delete != null)
            {
                delete.IsDeleted = true;
                delete.DeletionTime = DateTime.Now;
                delete.DeleterUserId = AbpSession.UserId;
                delete.TrangThai = 1;
                _repository.Update(delete);
                result = ObjectMapper.Map<NguonKhachDto>(delete);
            }
            return result;
        }

        public async Task<DM_NguonKhach> GetNguonKhachDetail(Guid Id)
        {
            var nguonKhach = await _repository.GetAsync(Id);
            return nguonKhach;
        }
        public async Task<PagedResultDto<DM_NguonKhach>> GetAll(PagedNguonKhachResultRequestDto input)
        {
            PagedResultDto<DM_NguonKhach> ListResultDto = new PagedResultDto<DM_NguonKhach>();
            var lstData = await _repository.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false).OrderByDescending(x => x.CreationTime).ToListAsync();
            ListResultDto.TotalCount = lstData.Count;
            if (!string.IsNullOrEmpty(input.Keyword))
            {
                lstData = lstData.Where(x => x.TenNguon.Contains(input.Keyword) || x.MaNguon.Contains(input.Keyword)).ToList();
            }
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;

            ListResultDto.Items = lstData.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            return ListResultDto;
        }
    }

}
